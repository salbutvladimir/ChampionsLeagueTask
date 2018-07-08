using HTEC.ChampionsLeague.Utils.Constants;
using HTEC.ChampionsLeague.Utils.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HTEC.ChampionsLeague.Utils.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            this.next = next;
            this.logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(context, ex);
            }
        }

        private void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log exception
            logger.LogException(exception);

            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            if (exception is KeyNotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is ArgumentException) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { message = exception.Message });

            using (var writer = new StreamWriter(context.Response.Body))
            {
                context.Response.StatusCode = (int)code;
                context.Response.ContentType = ProjectConstants.ApplicationJson;
                writer.WriteAsync(result);
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
