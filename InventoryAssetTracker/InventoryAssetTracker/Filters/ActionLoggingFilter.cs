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
//referehttps://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.filters.iasyncactionfilter?view=aspnetcore-10.0
namespace InventoryAssetTracker.Filters
{
    public class ActionLoggingFilter : IAsyncActionFilter
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        /// <summary>
        /// gets an instance of the action logging filter with the specified service scope factory for dependency injection.
        /// </summary>
        /// <param name="serviceScopeFactory"></param>
        public ActionLoggingFilter(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        /// <summary>
        /// pass the user action to the next filter and log the action details if no exception occurs.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //await to the action finish
            ActionExecutedContext executedContext = await next();
            //action have error don't record
            if (executedContext.Exception is not null && !executedContext.ExceptionHandled)
            {
                return;
            }
            //get the current action descriptor, if it's null, return without logging
            ControllerActionDescriptor? actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            if (actionDescriptor == null)
            {
                return;
            }
            //get the user name and user id from the current http context, if the user is not authenticated, use "Anonymous" as the username and skip logging if user id is not valid.
            string username = context.HttpContext.User.Identity?.IsAuthenticated == true
                ? context.HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? "Unknown"
                : "Anonymous";
            string? userIdClaim = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //get id
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return;
            }
            //get the response status code and construct the log details, then save the log record to the database using a new scope for the user context.
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
               
            }
        }
    }
}