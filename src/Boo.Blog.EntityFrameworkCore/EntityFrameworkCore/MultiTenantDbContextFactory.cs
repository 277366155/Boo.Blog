using System.IO;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Boo.Blog.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class MultiTenantDbContextFactory : IDesignTimeDbContextFactory<MultiTenantDbContext>
    {
        public MultiTenantDbContext CreateDbContext(string[] args)
        {
            //BlogEfCoreEntityExtensionMappings.Configure();

            //var configuration = BuildConfiguration();

            //var builder = new DbContextOptionsBuilder<BlogDbContext>()
            //    .UseSqlServer(configuration.GetConnectionString("Default"));

            var builder = new DbContextOptionsBuilder<MultiTenantDbContext>();
            switch (AppSettings.EnableDb)
            {
                default:
                case  DatabaseType.MYSQL:
                    builder.UseMySql(AppSettings.Root.GetConnectionString("MySql"), MySqlServerVersion.LatestSupportedServerVersion);
                    break;
                case DatabaseType.MSSQL:
                    builder.UseSqlServer(AppSettings.Root.GetConnectionString("MSSql"));
                    break;
            }

            return new MultiTenantDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Boo.Blog.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
