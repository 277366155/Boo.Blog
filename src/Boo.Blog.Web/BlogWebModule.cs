using System;
using Boo.Blog.EntityFrameworkCore;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Boo.Blog.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        typeof(BlogHttpApiModule),
        //typeof(BlogSwaggerModule),
        typeof(BlogEntityFrameworkCoreModule)
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
            //http请求
            context.Services.AddHttpClient();
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
            app.UseEndpoints(endPoint =>
            {
                endPoint.MapControllers();
            });
        }
    }
}
