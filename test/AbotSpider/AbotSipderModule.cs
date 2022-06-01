using AbotSpider.Crawlers;
using Boo.Blog.MongoDB;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace AbotSpider
{
    [DependsOn(typeof(AbpAutofacModule),
        typeof(AbpMongoDbModule))]
    public class AbotSipderModule:AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddCSRedisCore(AppSettings.Root.GetSection("Redis").Get<RedisHandlerOption>());

            context.Services.AddMongoDbContext<CrawlerMongoDbContext>(opt =>
            {
                opt.AddDefaultRepositories();
            });

            Configure<AbpUnitOfWorkDefaultOptions>(opt =>
            {
                opt.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            });
        }
    }
}
