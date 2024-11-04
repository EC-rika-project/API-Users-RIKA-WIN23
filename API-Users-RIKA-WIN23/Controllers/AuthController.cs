using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using API_Users_RIKA_WIN23.Infrastructure.Services;
using API_Users_RIKA_WIN23.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace API_Users_RIKA_WIN23.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IConfiguration configuration, AuthService authService, StatusCodeGenerator statusCodeSelector) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AuthService _authService = authService;
    private readonly StatusCodeGenerator _statusCodeSelector = statusCodeSelector;

    #region Token Authentication
    [Route("/api/JWT")]
    [HttpPost]
    //public IActionResult GetToken(string role, UserDto? user)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    if (user == null || string.IsNullOrWhiteSpace(user.UserName))
    //    {
    //        return BadRequest("Invalid user data.");
    //    }

    //    try
    //    {
    //        var claims = new List<Claim>();

    //        switch (role)
    //        {
    //            case "Admin":
    //                claims = new()
    //                {
    //                    new Claim(ClaimTypes.Name, user.UserName),
    //                    new Claim(ClaimTypes.Role, $"{role}"),
    //                    new Claim("Permission", "CanEditAllUsers")
    //                };
    //                break;
    //            case "User":
    //                claims = new()
    //                {
    //                    new Claim(ClaimTypes.Name, user.UserName),
    //                    new Claim(ClaimTypes.Role, $"{role}"),
    //                    new Claim("Permission", "CanEditSelf")
    //                };
    //                break;
    //            default:
    //                claims = new()
    //                {
    //                    new Claim(ClaimTypes.Name, "Guest"),
    //                    new Claim(ClaimTypes.Role, "Guest"),
    //                    new Claim("Permission", "CanNotEdit")
    //                };
    //                break;
    //        }


    //        var tokenHandler = new JwtSecurityTokenHandler();
    //        var tokenDescriptor = new SecurityTokenDescriptor
    //        {
    //            Subject = new ClaimsIdentity(claims),
    //            Issuer = _configuration["JWT:Issuer"],
    //            Audience = _configuration["JWT:Audience_DEV"],
    //            Expires = DateTime.Now.AddMinutes(30),
    //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[$"JWT:{role}"]!)), SecurityAlgorithms.HmacSha512),
    //        };

    //        var token = tokenHandler.CreateToken(tokenDescriptor);
    //        return Ok(tokenHandler.WriteToken(token));
    //    }
    //    catch (Exception ex)
    //    {
    //        return BadRequest(ex.Message);
    //    }
    //}
    #endregion

    #region SignIn
    [Route("/api/SignIn")]
    [HttpPost]
    public async Task<IActionResult> SignInAsync(SignInDto user)
    {
        if (ModelState.IsValid)
        {
            // Create a service for the following logic:
            var result = await _authService.SignInUserAsync(user);
            if (result != null)
            {
                return Ok(result);
            }
        }

        return BadRequest();
    }
    #endregion
}