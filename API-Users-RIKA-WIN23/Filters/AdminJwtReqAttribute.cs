using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API_Users_RIKA_WIN23.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]  
public class AdminJwtReqAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        var JWTSignature = configuration!.GetValue<string>("JwtKey");

        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (string.IsNullOrEmpty(JWTSignature))
        {
            context.Result = new ObjectResult("Internal server error occurred, failed to read internal JWT signature")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            return;
        }

        if (ValidateToken(token))
        {
            await next();
        }
        else
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        bool ValidateToken(string token)
        {
            try
            {
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

                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
                var permissionClaim = principal.FindFirst("permission")?.Value;

                if (permissionClaim == "CanEditAllUsers")
                {
                    return true;                        
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
