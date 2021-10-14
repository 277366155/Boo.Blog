using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TangPoem.Core;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace TangPoem.EF
{
    [DependsOn(typeof(PoemCoreModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class PoemEFModule : AbpModule
    {
        public bool SkipDbContextRegistration { get; set; }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            if (!SkipDbContextRegistration)
            {
                context.Services.AddAbpDbContext<PoemDbContext>(options =>
                {
                    options.AddDefaultRepositories(includeAllEntities: true);
                });

                var option = context.Services.ExecutePreConfiguredActions<ConnectionStringOptions>();
                context.Services.AddDbContext<PoemDbContext>(opt => opt.UseMySql(option.ConnectionString, ServerVersion.AutoDetect(option.ConnectionString)));
            }
            //Configure<AbpUnitOfWorkDefaultOptions>(opt =>
            //{
            //    opt.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            //});
        }
    }
}
