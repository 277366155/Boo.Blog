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
            //�����֤
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
            //��֤��Ȩ
            context.Services.AddAuthorization();
            //ע��IHttpClientFactory
            context.Services.AddHttpClient();
            /*
             * ע��һ��tenantĬ��ʵ�����������м���в�ѯtoken��Ϣ������tenant������Ϣ��
             * ������BlogDbContextProvider����ܵ���ִ��ʱ����ȡ����Ӧ��BlogDbContext
            **/
            context.Services.AddScoped<ITenant, Tenant>();
            context.Services.AddScoped<ITenantRepository, TenantRepository>();
            //context.Services.AddSingleton<ILogger>();
            //�Ƴ�AbpExceptionFilter
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
            //����cookie��֤���������ڶ��⻧DBContext������
            app.AddIdentityHandler();
            //�����֤���������UseRouting()��UseEndpoints()֮��
            app.UseAuthentication();
            //��֤��Ȩ
            app.UseAuthorization();

            app.UseEndpoints(endPoint =>
            {
                endPoint.MapControllers();
            });
        }
    }
}
