using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using TangPoem.Application.TestBase;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.Modularity;
using Xunit;

namespace TangPoem.EF.Test
{
    [DependsOn(typeof(PoemEFModule),
        typeof(PoemTestBaseModule),
        typeof(AbpEntityFrameworkCoreSqliteModule))]
    public class PoemEFTestModule : AbpModule
    {
        SqliteConnection _sqliteConnection;
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureInMemorySqlite(context.Services);
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
