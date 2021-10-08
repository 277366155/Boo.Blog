using Boo.Blog.Application;
using Boo.Blog.Application.HelloWorld;
using Boo.Blog.ToolKits.Interceptor;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Services;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpIdentityApplicationModule),
        typeof(BlogDomainModule),
        typeof(BlogApplicationCachingModule)
        )]
    public class BlogApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<BlogApplicationModule>(validate: true);
                opt.AddProfile<BlogAutoMapperProfile>(validate: true);
            });
            context.Services.AddSingleton(typeof(UnitOfWorkInterceptor));
            context.Services.OnRegistred(register =>
            {
                // 添加拦截器
                if (typeof(ApplicationService) == register.ImplementationType.BaseType)
                {
                register.Interceptors.Add<UnitOfWorkInterceptor>();
                }
            });
        }

    }
}
