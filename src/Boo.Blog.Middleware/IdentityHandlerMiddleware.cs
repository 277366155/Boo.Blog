using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.ToolKits.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Boo.Blog.Middleware
{
    public static class IdentityHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder AddIdentityHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<IdentityHandlerMiddleware>();
            return applicationBuilder;
        }
    }

    public class IdentityHandlerMiddleware
    {
        readonly RequestDelegate _next;
        //readonly ITenant _tenant;
        readonly IRedisHandler _redisHandler;
        readonly string tokenKey = "tenantToken";
        public IdentityHandlerMiddleware(RequestDelegate next,
            //ITenant tenant,
            IRedisHandler redisHandler)
        {
            if (next == null)
                throw new ArgumentNullException(nameof(next));
            _next = next;
            
            //不能在类中使用字段接收tenant实例，这样做无法给容器内的实例属性赋值
            //_tenant = tenant;
            _redisHandler = redisHandler;
        }

        public async Task InvokeAsync(HttpContext context, ITenant tenant)
        {
            var cookies = context.Request.Cookies;
            var token = string.Empty;
            if (cookies == null || !cookies.TryGetValue(tokenKey, out token))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
            var tenantInfo = await _redisHandler.GetAsync<Tenant>(token);
            if (tenantInfo == null || tenantInfo.IsDeleted)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
            tenant.Id = tenantInfo.Id;
            tenant.TenantName = tenantInfo.TenantName;
            tenant.DatabaseServerId = tenantInfo.DatabaseServerId;
            //先验证身份，再执行action
            await _next.Invoke(context);
        }
    }
}
