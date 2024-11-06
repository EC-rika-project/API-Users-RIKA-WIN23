using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Users_RIKA_WIN23.Filters;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();
        var apiKey = configuration!.GetValue<string>("ApiSecret");

        bool hasKey = context.HttpContext.Request.Headers.TryGetValue("key", out var clientKey);

        if (!hasKey)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!apiKey!.Equals(clientKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
