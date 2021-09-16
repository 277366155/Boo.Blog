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
            //context.Services.AddAbpDbContext<BlogDbContext>(options =>
            //{
            //    /* Remove "includeAllEntities: true" to create
            //     * default repositories only for aggregate roots */
            //    options.AddDefaultRepositories(includeAllEntities: true);
            //});

            //Configure<AbpDbContextOptions>(options =>
            //{
            //    /* The main point to change your DBMS.
            //     * See also BlogMigrationsDbContextFactory for EF Core tooling. */
            //    options.UseSqlServer();
            //});
        }
    }
}
