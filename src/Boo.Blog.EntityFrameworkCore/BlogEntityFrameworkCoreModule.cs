using Boo.Blog.ToolKits.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace Boo.Blog.EntityFrameworkCore
{
    [DependsOn(
        typeof(BlogDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpEntityFrameworkCoreMySQLModule)
        )]
    public class BlogEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            //BlogEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //注意，这里自定义的IDbContextProvider需要手动注入
            context.Services.AddScoped<IDbContextProvider<BlogDbContext>, BlogDbContextProvider>();
            Configure<AbpDbContextOptions>(options =>
            {
                switch (AppSettings.EnableDb)
                {
                    default:
                    case "MYSQL":
                        options.UseMySQL();
                        break;
                    case "MSSQL":
                        options.UseSqlServer();
                        break;
                }
            });
            context.Services.AddAbpDbContext<MultiTenantDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            context.Services.AddAbpDbContext<BlogDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
        }
    }
}
