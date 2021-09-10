using Boo.Blog.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(BlogEntityFrameworkCoreTestModule)
        )]
    public class BlogDomainTestModule : AbpModule
    {

    }
}