// before .NET 8 we use this for global handling exception :

using System.Net;
using Contracs;
using Entities.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;

namespace TodoAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        app.UseExceptionHandler(appError => 
            appError.Run(async context => { 
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError($"Internal Server Error: {contextFeature.Error}");
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error",
                    }.ToString());
                }
            })
            );
    }
}