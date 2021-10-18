using Boo.Blog.ToolKits.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangPoem.Application;
using TangPoem.EF;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace TangPoem.Web
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule),
        //typeof(PoemEFModule),
        typeof(PoemApplicationModule))]
    public class PoemWebModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var conn = new MySqlConnection(AppSettings.Root.GetConnectionString("MySql"));
            context.Services.Configure<AbpDbContextOptions>(opt =>
            {
                opt.Configure(c =>
                {
                    c.DbContextOptions.UseMySql(conn, ServerVersion.AutoDetect(conn));
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseConfiguredEndpoints();
        }
    }
}
