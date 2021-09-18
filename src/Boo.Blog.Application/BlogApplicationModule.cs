using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpIdentityApplicationModule),
        typeof(BlogDomainModule)
        )]
    public class BlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt=> {
                opt.AddMaps<BlogApplicationModule>(validate:true);
                opt.AddProfile<BlogAutoMapperProfile>(validate:true);
            });
        }
    }
}
