using System;
using TangPoem.Core;
using TangPoem.EF;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace TangPoem.Application
{
    [DependsOn(typeof(PoemEFModule),
        typeof(AbpAutoMapperModule))]
    public class PoemApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(opt =>
            {
                opt.AddProfile<PoemApplicationAutoMapperProfile>(validate:true);
            });
        }
    }
}
