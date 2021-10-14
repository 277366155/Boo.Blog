using Boo.Blog.ToolKits.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TangPoem.EF;
using Volo.Abp.Modularity;

namespace ConsolePro
{
    [DependsOn(typeof(PoemEFModule))]
    public class ConsoleClientModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<ConnectionStringOptions>(opt => opt.ConnectionString = AppSettings.Root.GetConnectionString("MySql"));
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //Configuration.DefaultNameOrConnectionString = "Server=127.0.0.1;database=Poem;UID=sa;PWD=1qaz@WSX";
            //var conn = AppSettings.Root.GetConnectionString("MySql");
            //Configure<DbContextOptionsBuilder<PoemDbContext>>(opt => opt.UseMySql(conn, MySqlServerVersion.AutoDetect(conn)));

            context.Services.OnRegistred(c => Console.WriteLine(c.ImplementationType.FullName));
        }
    }
}
