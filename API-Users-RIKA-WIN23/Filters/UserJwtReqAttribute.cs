using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API_Users_RIKA_WIN23.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UserJwtReqAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        var JWTSignature = configuration!.GetValue<string>("JwtKey");
        
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var hasEmail = context.ActionArguments.TryGetValue("email", out var queryEmail) && queryEmail is string emailFromRoute;
        var hasId = context.ActionArguments.TryGetValue("userId", out var queryId) && queryId is string idFromRoute;


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
                var id = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                var nameClaim = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
                var roleClaim = principal.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

                if (permissionClaim == "CanEditAllUsers" && roleClaim == "admin")
                {
                    return true;
                }

                if (hasEmail && permissionClaim == "CanEditSelf" && nameClaim == queryEmail?.ToString())
                {
                    return true;
                }

                if (hasId && permissionClaim == "CanEditSelf" && id == queryId?.ToString())
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