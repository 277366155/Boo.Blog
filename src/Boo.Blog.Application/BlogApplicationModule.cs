using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(AbpIdentityApplicationModule)
        )]
    public class BlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
