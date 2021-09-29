using Boo.Blog.Response;
using Boo.Blog.ToolKits.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Boo.Blog.Middleware
{
    public class GloableExceptionHandlerMiddleware
    {
        readonly RequestDelegate _next;
        public GloableExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandlerAsync(context,ex.Message);
            }

            if (context.Response.StatusCode != StatusCodes.Status200OK)
            {
                object message = "";
                Enum.TryParse(typeof(HttpStatusCode),context.Response.StatusCode.ToString(),out message);
                await ExceptionHandlerAsync(context, message.ToString());
            }
        }

        private async Task ExceptionHandlerAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json;charset=utf-8";
            var data = ResponseResult.IsFail(message);
            await context.Response.WriteAsync(data.ToJson());
        }
    }
}
