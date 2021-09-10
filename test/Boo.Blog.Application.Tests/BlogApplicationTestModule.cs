using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(BlogApplicationModule),
        typeof(BlogDomainTestModule)
        )]
    public class BlogApplicationTestModule : AbpModule
    {

    }
}