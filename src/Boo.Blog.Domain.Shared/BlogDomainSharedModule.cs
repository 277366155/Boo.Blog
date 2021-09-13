using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(AbpIdentityDomainModule)
        )]
    public class BlogDomainSharedModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
