using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Boo.Blog.Application
{
    [DependsOn(
        typeof(AbpCachingModule),
        typeof(BlogDomainModule)
        )]
    public class BlogApplicationCachingModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
        }
    }
}
