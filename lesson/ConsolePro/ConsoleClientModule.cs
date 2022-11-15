using Boo.Blog.ToolKits.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using TangPoem.Application;
using TangPoem.EF;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ConsolePro
{
    [DependsOn(typeof(AbpAutofacModule),
        typeof(PoemApplicationModule))]
    public class ConsoleClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configration = context.Services.GetConfiguration();
            var conn = new MySqlConnection(configration.GetConnectionString("MySql"));
            context.Services.Configure<AbpDbContextOptions>(opt =>
            {
                opt.Configure(c =>
                {
                    c.DbContextOptions.UseMySql(conn, ServerVersion.AutoDetect(conn));
                });
            });
        }
    }
}
