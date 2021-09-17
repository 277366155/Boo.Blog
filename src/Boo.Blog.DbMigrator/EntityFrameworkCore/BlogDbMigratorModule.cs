using Boo.Blog.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Boo.Blog.DbMigrator.EntityFrameworkCore
{
    [DependsOn(
        //typeof(AbpAutofacModule),
        typeof(BlogEntityFrameworkCoreModule)
        )]
    public class BlogDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
            context.Services.AddAbpDbContext<BlogMigrationsDbContext>();
        }
    }
}
