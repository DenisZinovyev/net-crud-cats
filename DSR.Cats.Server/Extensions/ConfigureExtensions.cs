using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DSR.Cats.Server.WebApi.Extensions
{
    public static class ConfigureExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
        {
            app.UseExceptionHandler(appError => appError.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                if (exception != null)
                {
                    var problemDetails = new ProblemDetails();
                    if (exception is BadHttpRequestException badHttpRequestException)
                    {
                        problemDetails.Title = "Invalid request";
                        problemDetails.Status = badHttpRequestException.StatusCode;
                        problemDetails.Detail = badHttpRequestException.Message;
                    }
                    else
                    {
                        problemDetails.Title = "An unexpected error occurred!";
                        problemDetails.Status = 500;
                        problemDetails.Detail = exception.Message.ToString();
                    }

                    logger.LogError($"Something went wrong: {exception}");
                    context.Response.StatusCode = problemDetails.Status.Value;
                    context.Response.ContentType = "application/problem+json";
                    var result = JsonConvert.SerializeObject(problemDetails);
                    await context.Response.WriteAsync(result);
                }
            }));
        }

        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}
