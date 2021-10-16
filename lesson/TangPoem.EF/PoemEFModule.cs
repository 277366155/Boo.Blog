using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TangPoem.Core;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace TangPoem.EF
{
    [DependsOn(typeof(PoemCoreModule),
        typeof(AbpEntityFrameworkCoreMySQLModule))]
    public class PoemEFModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<PoemDbContext>(options =>
            {
                    //为所有实体生成默认的Repository
                    options.AddDefaultRepositories(includeAllEntities: true);
            });
            Configure<AbpDbContextOptions>(opt => opt.UseMySQL());
        }
    }
}
