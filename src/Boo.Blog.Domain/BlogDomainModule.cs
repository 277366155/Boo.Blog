using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(BlogDomainSharedModule),
        typeof(AbpIdentityDomainModule)
        )]
    public class BlogDomainModule:AbpModule
    {
    }
}
