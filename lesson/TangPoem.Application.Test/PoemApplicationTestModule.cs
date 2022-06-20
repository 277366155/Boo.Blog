using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TangPoem.Application.TestBase;
using TangPoem.EF;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace TangPoem.Application.Test
{
    [DependsOn(typeof(PoemTestBaseModule),
        typeof(PoemApplicationModule),
        typeof(AbpEntityFrameworkCoreSqliteModule))]
    public class PoemApplicationTestModule : AbpModule
    {
        SqliteConnection _sqliteConnection;
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureInMemorySqlite(context.Services);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            //程序初始化阶段，执行种子数据插入
            AsyncHelper.RunSync(async () =>
            {
                using var scope = context.ServiceProvider.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IDataSeeder>().SeedAsync();
            });
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            _sqliteConnection.Dispose();
        }

        private void ConfigureInMemorySqlite(IServiceCollection services)
        {
            _sqliteConnection = CreateDbAndGetConnection();
            services.Configure<AbpDbContextOptions>(opt =>
            {
                opt.Configure(context =>
                {
                    context.DbContextOptions.UseSqlite(_sqliteConnection);
                });
            });
        }
        private SqliteConnection CreateDbAndGetConnection()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            conn.Open();
            var opt = new DbContextOptionsBuilder<PoemDbContext>()
                .UseSqlite(conn)
                .Options;
            using var context = new PoemDbContext(opt);
            context.GetService<IRelationalDatabaseCreator>().CreateTables();
            return conn;
        }
    }
}
