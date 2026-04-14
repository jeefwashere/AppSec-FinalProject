using InventoryAssetTracker.Data;
using InventoryAssetTracker.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
/*
 * FILE : ActionLoggingFilter.cs
 * PROGRAMMER : Name(s): Josiah Williams, Jeff, Gao Ricardo
 * DESCRIPTION : Logs action executions for monitoring and debugging purposes.
 */
//reference https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0
namespace InventoryAssetTracker.Filters
{
    public class ActionLoggingFilter : IAsyncActionFilter
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ActionLoggingFilter(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext executedContext = await next();

            if (executedContext.Exception is not null && !executedContext.ExceptionHandled)
            {
                return;
            }

            ControllerActionDescriptor? actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor == null)
            {
                return;
            }

            string username = context.HttpContext.User.Identity?.IsAuthenticated == true
                ? context.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? "Unknown"
                : "Anonymous";
            string? userIdClaim = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return;
            }

            int statusCode = context.HttpContext.Response.StatusCode;
            string actionName = $"{context.HttpContext.Request.Method} {actionDescriptor.ControllerName}.{actionDescriptor.ActionName}";
            string details = $"{context.HttpContext.Request.Path} returned {statusCode}";

            try
            {
                using IServiceScope scope = serviceScopeFactory.CreateScope();
                UserContext userContext = scope.ServiceProvider.GetRequiredService<UserContext>();

                Log log = new Log
                {
                    Action = actionName,
                    Details = details,
                    Username = username,
                    UserId = userId
                };

                userContext.Logs.Add(log);
                await userContext.SaveChangesAsync();
            }
            catch
            {
                // Logging should not block the original request flow.
            }
        }
    }
}