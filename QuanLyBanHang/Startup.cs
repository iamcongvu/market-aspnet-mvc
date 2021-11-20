using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using QuanLyBanHang.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyBanHang
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Hello Vtc");
                await next.Invoke();
                await context.Response.WriteAsync("Return Hello Vtc");
            });

            app.UseMiddleware<SimpleMiddleware>();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello from 2nd delegate.");
            });
        }
    }
}