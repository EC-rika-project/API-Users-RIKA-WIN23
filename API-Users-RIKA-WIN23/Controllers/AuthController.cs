using API_Users_RIKA_WIN23.Filters;
using API_Users_RIKA_WIN23.Infrastructure.Context;
using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Entities;
using API_Users_RIKA_WIN23.Infrastructure.Factories;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API_Users_RIKA_WIN23.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(DataContext context, IConfiguration configuration, UserManager<UserEntity> userManager, AuthService authService, StatusCodeGenerator statusCodeGenerator) : ControllerBase
{
    private readonly DataContext _context = context;
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AuthService _authService = authService;
    private readonly StatusCodeGenerator _statusCodeGenerator = statusCodeGenerator;

    [ApiKey]
    #region SignIn
    [Route("/api/SignIn")]
    [HttpPost]
    public async Task<IActionResult> SignInAsync(SignInDto signInDto)
    {
        if (ModelState.IsValid)
        {
            var signInResult = await _authService.SignInUserAsync(signInDto);
            if (signInResult.StatusCode == Infrastructure.Utilities.StatusCode.OK)
            {
                var JwtResult = await GenerateJwtAsync(signInDto);
                return _statusCodeGenerator.HttpSelector(JwtResult);
            }

            return _statusCodeGenerator.HttpSelector(signInResult);
        }

        return BadRequest();
    }
    #endregion

    #region Generate JWT
    private async Task<ResponseResult> GenerateJwtAsync(SignInDto signInDto)
    {
        try
        {
            var userEntity = await _context.Users
            .Include(x => x.Profile)
            .Include(x => x.Address)
            .Include(x => x.WishList)
            .Include(x => x.ShoppingCarts)
            .FirstOrDefaultAsync(x => x.UserName == signInDto.Email);

            if (userEntity == null)
            {
                return ResponseFactory.Error("User not found after sign in");
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtKey")!)), SecurityAlgorithms.HmacSha512),
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
    #endregion
}