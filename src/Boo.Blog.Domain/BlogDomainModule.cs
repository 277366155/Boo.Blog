using System;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog.Domain
{
    [DependsOn(
        typeof(AbpIdentityDomainModule)
        )]
    public class BlogDomainModule:AbpModule
    {
    }
}
