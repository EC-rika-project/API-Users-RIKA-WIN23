using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Users_RIKA_WIN23.Infrastructure.Services;

public class AuthService(DataContext context, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, IConfiguration configuration)
{
    private readonly DataContext _context = context;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    #region Sign In
    public async Task<ResponseResult> SignInUserAsync(SignInDto signInDto)
    {
        try
        {
            var signInResult = await _signInManager.PasswordSignInAsync(signInDto.Email, signInDto.Password, signInDto.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return ResponseFactory.Ok();
            }

            return ResponseFactory.Error($"Sign in failed, check your credentials and try again.");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError($"User could not be signed in: {ex.Message}");
        }
    }
    #endregion

    #region Generate JWT
    public async Task<ResponseResult> GenerateJwtAsync(SignInDto signInDto)
    {
        try
        {
            var symmetricalKey = _configuration.GetValue<string>("JwtKey");

            var userEntity = await _context.Users
            .Include(x => x.Profile)
            .Include(x => x.Address)
            .Include(x => x.WishList)
            .Include(x => x.ShoppingCarts)
            .FirstOrDefaultAsync(x => x.UserName == signInDto.Email);

            if (userEntity == null)
            {
                return ResponseFactory.NotFound("User not found after sign in");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userEntity.Email!),
                new Claim(ClaimTypes.NameIdentifier, userEntity.Id),
            };

            var roles = await _userManager.GetRolesAsync(userEntity);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (roles.Contains("admin"))
            {
                claims.Add(new Claim("permission", "CanEditAllUsers"));
            }
            else if (roles.Contains("user"))
            {
                claims.Add(new Claim("permission", "CanEditSelf"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration.GetValue<string>("Issuer"),
                Audience = _configuration.GetValue<string>("Audience"),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricalKey!)), SecurityAlgorithms.HmacSha512),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var JWT = new JwtDto
            {
                JWT = tokenHandler.WriteToken(token)
            };
            return ResponseFactory.Created(JWT);
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError(ex.Message);
        }
    }

    public async Task<ResponseResult> GenerateResetJwtAsync(string email)
    {
        try
        {
            var symmetricalKey = _configuration.GetValue<string>("JwtKey");
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.UserName == email);

            if (userEntity == null)
            {
                return ResponseFactory.NotFound("User not found");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userEntity.Email!),
                new Claim(ClaimTypes.NameIdentifier, userEntity.Id),
            };

            var roles = await _userManager.GetRolesAsync(userEntity);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (roles.Contains("admin"))
            {
                claims.Add(new Claim("permission", "CanEditAllUsers"));
            }
            else if (roles.Contains("user"))
            {
                claims.Add(new Claim("permission", "CanEditSelf"));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration.GetValue<string>("Issuer"),
                Audience = _configuration.GetValue<string>("Audience"),
                Expires = DateTime.Now.AddMinutes(10), //Shorter time for resetting password than for a user to stay logged in.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricalKey!)), SecurityAlgorithms.HmacSha512),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var JWT = new 

            {
                JWT = tokenHandler.WriteToken(token)
            };
            return ResponseFactory.Created(JWT);
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError(ex.Message);
        }
    }
    #endregion

    #region Validate reset token (JWT)
    public async Task<ResponseResult> ValidateResetTokenAsync(ResetPasswordDto resetDto)
    {
        if (resetDto == null)
        {
            return ResponseFactory.Error("resetDto was null");
        }

        var jwtResult = await ValidateJWTAync(resetDto);
        if (jwtResult.StatusCode != StatusCode.OK)
        {
            return jwtResult;
        }

        return await ResetPasswordAsync(jwtResult.Message!, resetDto.NewPassword);
    }
    #endregion

    private async Task<ResponseResult> ValidateJWTAync(ResetPasswordDto resetDto)
    {
        try
        {
            var JWTSignature = _configuration!.GetValue<string>("JwtKey");

            if (string.IsNullOrEmpty(JWTSignature))
            {
                return ResponseFactory.InternalServerError("Internal server error occurred, failed to read internal JWT signature");
            }

            var handler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration!.GetValue<string>("Issuer"),
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSignature!)),
                ClockSkew = TimeSpan.Zero.Add(TimeSpan.FromSeconds(5))
            };

            var principal = handler.ValidateToken(resetDto.JWT, validationParameters, out var validatedToken);
            var id = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (id != null)
            {
                return ResponseFactory.Ok(id);
            }

            return ResponseFactory.Unauthorized("Token invalid");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError(ex.Message);
        }
    }

    private async Task<ResponseResult> ResetPasswordAsync(string userId, string newPassword)
    {
        try
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (userEntity == null)
            {
                return ResponseFactory.NotFound("User not found from JWT claim");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userEntity);
            var result = await _userManager.ResetPasswordAsync(userEntity, resetToken, newPassword);
            if (result.Succeeded)
            {
                return ResponseFactory.Ok("Password reset succesful");
            }

            return ResponseFactory.InternalServerError("Password could not be reset, please try again. Contact support if error persists");
        }
        catch (Exception ex)
        {
            return ResponseFactory.InternalServerError(ex.Message);
        }
    }
}