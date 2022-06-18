using System;
using System.Linq;
using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.EntityFrameworkCore;
using Boo.Blog.Middleware;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Boo.Blog.Domain.MultiTenant.IRepositories;
using Boo.Blog.EntityFrameworkCore.Repositories.MultiTenant;

namespace Boo.Blog.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(MiddlewareModule),
        typeof(AbpAutofacModule),
        typeof(BlogHttpApiModule),
        //typeof(BlogSwaggerModule),        
        typeof(BlogEntityFrameworkCoreModule)
    //typeof(BlogMongoDbModule)
    )]
    public class BlogWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwagger();
            //身份验证
            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.FromSeconds(60),
                        ValidateIssuerSigningKey = true,
                        ValidAudience = AppSettings.Root["Jwt:Domain"],
                        ValidIssuer = AppSettings.Root["Jwt:Domain"],
                        IssuerSigningKey = new SymmetricSecurityKey(AppSettings.Root["Jwt:SecurityKey"].GetBytes())
                    };
                });
            //认证授权
            context.Services.AddAuthorization();
            //注入IHttpClientFactory
            context.Services.AddHttpClient();
            /*
             * 注入一个tenant默认实例，用于在中间件中查询token信息，并绑定tenant基础信息。
             * 进而在BlogDbContextProvider被框架调用执行时，获取到对应的BlogDbContext
            **/
            context.Services.AddScoped<ITenant, Tenant>();
            context.Services.AddScoped<ITenantRepository, TenantRepository>();
            //context.Services.AddSingleton<ILogger>();
            //移除AbpExceptionFilter
            Configure<MvcOptions>(opt =>
            {
                var filterMetadata = opt.Filters.FirstOrDefault(a => a is ServiceFilterAttribute attribute && attribute.ServiceType.Equals(typeof(AbpExceptionFilter)));
                opt.Filters.Remove(filterMetadata);
            });
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // base.OnApplicationInitialization(context);

            var app = context.GetApplicationBuilder().UseSwagger().UseSwaggerUI();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            //app.UseMiddleware<GloableExceptionHandlerMiddleware>();
            app.UseMiddleware<SerilogHandlerMiddleware>();
            //新增cookie验证，并可用于多租户DBContext的生成
            app.AddIdentityHandler();
            //身份验证，必须放在UseRouting()与UseEndpoints()之间
            app.UseAuthentication();
            //认证授权
            app.UseAuthorization();

            app.UseEndpoints(endPoint =>
            {
                endPoint.MapControllers();
            });
        }
    }
}
