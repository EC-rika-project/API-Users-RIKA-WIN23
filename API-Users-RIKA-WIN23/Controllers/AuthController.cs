using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Users_RIKA_WIN23.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    #region Token Authentication
    [HttpPost]
    public IActionResult GetToken(string role, UserDto? user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        if (user == null || string.IsNullOrWhiteSpace(user.UserName))
        {
            return BadRequest("Invalid user data.");
        }

        try
        {
            var claims = new List<Claim>();

            switch (role)
            {
                case "Admin":
                    claims = new()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, $"{role}"),
                        new Claim("Permission", "CanEditAllUsers")
                    };
                break;
                case "User":
                    claims = new()
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, $"{role}"),
                        new Claim("Permission", "CanEditSelf")
                    };
                break;
                default:
                    claims = new()
                    {
                        new Claim(ClaimTypes.Name, "Guest"),
                        new Claim(ClaimTypes.Role, "Guest"),
                        new Claim("Permission", "CanNotEdit")
                    };
                    break;
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience_DEV"],
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[$"JWT:{role}"]!)), SecurityAlgorithms.HmacSha512),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(tokenHandler.WriteToken(token));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    #endregion

    #region SignIn
    [Route("/signin")]
    [HttpGet]
    public async Task<IActionResult> SignIn(UserDto user)
    {
        if (ModelState.IsValid)
        {
            // Create a service for the following logic:
            var result = true;//_signInService.SignInUserAsync(user);
            if (result)
            {
                return Ok(result);   
            }
        }

        return Unauthorized();
    }
    #endregion
}