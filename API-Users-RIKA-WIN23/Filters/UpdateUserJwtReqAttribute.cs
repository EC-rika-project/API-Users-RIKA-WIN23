using API_Users_RIKA_WIN23.Infrastructure.DTOs;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API_Users_RIKA_WIN23.Filters;

public class UpdateUserJwtReqAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        //var JWTSignature = configuration!.GetValue<string>("JwtKey");

        //var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //var userId = string.Empty;
        //var request = context.HttpContext.Request;
        //request.EnableBuffering();
        //using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        //var body = await reader.ReadToEndAsync();
        //request.Body.Position = 0;


        //foreach (var parameter in context.ActionDescriptor.Parameters)
        //{
        //    switch (parameter.ParameterType)
        //    {
        //        case Type type when type == typeof(UserDto):
        //            userId = await GetUserID<UserDto>(body);
        //            break;
        //        case Type type when type == typeof(UserProfileDto):
        //            userId = await GetUserID<UserProfileDto>(body);
        //            break;
        //        case Type type when type == typeof(UserAddressDto):
        //            userId = await GetUserID<UserProfileDto>(body);
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //async Task<string> GetUserID<T>(string body)
        //{
        //    var userObject = JsonConvert.DeserializeObject<T>(body);
        //    if (userObject != null)
        //    {
        //        var idProperty = userObject.GetType().GetProperty("Id") ?? userObject.GetType().GetProperty("UserId");
        //        if (idProperty != null)
        //        {
        //            return (string?)idProperty.GetValue(userObject) ?? string.Empty;
        //        }                
        //    }
        //    return userId;
        //}

        //if (string.IsNullOrEmpty(userId))
        //{
        //    context.Result = new ObjectResult("Internal server error occurred, failed to deserialize userId for authentication")
        //    {
        //        StatusCode = StatusCodes.Status500InternalServerError
        //    };
        //    return;
        //}

        //if (token == null)
        //{
        //    context.Result = new UnauthorizedResult();
        //    return;
        //}

        //if (string.IsNullOrEmpty(JWTSignature))
        //{
        //    context.Result = new ObjectResult("Internal server error occurred, failed to read internal JWT signature")
        //    {
        //        StatusCode = StatusCodes.Status500InternalServerError
        //    };
        //    return;
        //}

        //if (ValidateToken(token))
        //{
        //    await next();
        //}
        //else
        //{
        //    context.Result = new UnauthorizedResult();
        //    return;
        //}

        //bool ValidateToken(string token)
        //{
        //    try
        //    {
        //        var handler = new JwtSecurityTokenHandler();

        //        var validationParameters = new TokenValidationParameters
        //        {
        //            ValidateIssuer = true,
        //            ValidIssuer = configuration!.GetValue<string>("Issuer"),
        //            ValidateAudience = false,
        //            ValidateLifetime = true,
        //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSignature!)),
        //            ClockSkew = TimeSpan.Zero.Add(TimeSpan.FromSeconds(5))
        //        };

        //        var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
        //        var permissionClaim = principal.FindFirst("permission")?.Value;
        //        var id = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        //        var nameClaim = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
        //        var roleClaim = principal.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

        //        if (permissionClaim == "CanEditAllUsers" && roleClaim == "admin")
        //        {
        //            return true;
        //        }

        //        if (permissionClaim == "CanEditSelf" && id == userId)
        //        {
        //            return true;
        //        }

                //return false;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //    return false;
            //}
        //}
    }
}
