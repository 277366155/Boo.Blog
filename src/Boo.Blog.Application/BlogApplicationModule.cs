using Boo.Blog.Application;
using Boo.Blog.ToolKits.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Volo.Abp.Application.Services;
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
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddMaps<BlogApplicationModule>(validate: true);
                opt.AddProfile<BlogAutoMapperProfile>(validate: true);
            });
            var configuration = context.Services.GetConfiguration();
            context.Services.AddCSRedisCore(configuration.GetSection("Redis").Get<IEnumerable<RedisHandlerOption>>());
            //context.Services.AddSingleton(typeof(ApplicationInterceptor));
            context.Services.OnRegistred(register =>
            {
                // 添加拦截器
                if (register.ImplementationType.BaseType==typeof(ApplicationService) || register.ImplementationType.BaseType.Name.Equals("ServiceBase`3"))
                {
                    register.Interceptors.Add<ApplicationInterceptor>();
                }
            });
        }
    }
}
