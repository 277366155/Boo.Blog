using Boo.Blog.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Boo.Blog.DbMigrator.EntityFrameworkCore
{
    class BlogMigrationsDbContextFactory : IDesignTimeDbContextFactory<BlogMigrationsDbContext>
    {
        public BlogMigrationsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BlogMigrationsDbContext>()
                .UseMySql(AppSettings.Configuration.GetConnectionString("MySql"), MySqlServerVersion.LatestSupportedServerVersion);
            return new BlogMigrationsDbContext(builder.Options);
        }
        
    }
}
