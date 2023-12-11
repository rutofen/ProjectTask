using System.Diagnostics;
using System;
using System.Threading.Tasks; // Added for Task
using Microsoft.AspNetCore.Builder; // Added for IApplicationBuilder
using Microsoft.AspNetCore.Http; // Added for HttpContext
using Microsoft.Extensions.Logging; // Added for ILogger
namespace פרויקט
{
      public class LogMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext c)
        {
            var act = $"{c.Request.Path}.{c.Request.Method}";
            var sw = new Stopwatch();
            sw.Start();

            logger.LogInformation($"{act} started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
            await next.Invoke(c);
            logger.LogDebug($"{act} ended at {sw.ElapsedMilliseconds} ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        
        }
    }

    public static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}