using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Boo.Blog.Middleware
{
    public class CorsMiddleware
    {
        readonly RequestDelegate _next;
        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var headerKey = "Access-Control-Allow-Origin";
            if (!context.Response.Headers.ContainsKey(headerKey))
            {
                context.Response.Headers.Add(headerKey, "*");
            }
          await _next.Invoke(context);
        }
    }
}
