using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HttpContext = Microsoft.AspNetCore.Http.HttpContext;

namespace QuanLyBanHang.Middlewares
{
    public class SimpleMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public SimpleMiddleware(RequestDelegate next,ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("SimpleMiddleware");
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("Hello Vtc 2");
            await _next(context);
            await context.Response.WriteAsync("Return Hello Vtc 2");
        }
    }

    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SimpleMiddleware>();
        }
    }
}