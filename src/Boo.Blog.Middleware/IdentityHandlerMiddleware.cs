using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.Middleware.Attributes;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using Boo.Blog.ToolKits.Extensions;
using Boo.Blog.ToolKits.JwtUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Linq;
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
            var endPoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            //var attr = endPoint.Metadata.GetMetadata<IgnoreAuthenticationAttribute>();
            var attr = endPoint?.Metadata.GetMetadata<AuthorizeAttribute>();

            if (attr != null)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault();

                //鉴权的逻辑不能写在这里，会对所有接口都进行身份验证，不合理。
                if (string.IsNullOrWhiteSpace(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }

                var sercrityInfo = JwtUtil.JwtDecodeToken(token.Replace("Bearer ", ""), AppSettings.Root["Jwt:SecurityKey"],false);
                if (sercrityInfo != null && sercrityInfo.exp < DateTime.Now.AddMinutes(10).ToMilliTicks())
                { 
                    //todo:
                    //  1，重新生成jwt
                    //  2，redis缓存续期
                }
                var tenantInfo = await _redisHandler.GetAsync<Tenant>(sercrityInfo.tenantCode);
                if (tenantInfo == null || tenantInfo.IsDeleted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return;
                }
                tenant.Id = tenantInfo.Id;
                tenant.TenantName = tenantInfo.TenantName;
                tenant.DatabaseServerId = tenantInfo.DatabaseServerId;
            }

            await _next.Invoke(context);
        }
    }
}
