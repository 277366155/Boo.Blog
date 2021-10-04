using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.Extensions.Configuration;
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
            //base.ConfigureServices(context);
            context.Services.AddCSRedisCore(AppSettings.Root.GetSection("Redis").Get<RedisHandlerOption>());
        }
    }
}
