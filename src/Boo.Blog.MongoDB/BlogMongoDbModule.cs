using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Boo.Blog.MongoDB
{
    [DependsOn(typeof(BlogDomainModule),
        typeof(AbpMongoDbModule))]
    public class BlogMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BlogMongoDbContext>(opt =>
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
