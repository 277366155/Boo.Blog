﻿using Boo.Blog.Domain.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Boo.Blog.DbMigrator.EntityFrameworkCore
{
    class BlogMigrationsDbContextFactory : IDesignTimeDbContextFactory<BlogMigrationsDbContext>
    {
        public BlogMigrationsDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BlogMigrationsDbContext>();
            switch (AppSettings.EnableDb)
            {
                default:
                case "MYSQL":
                    builder.UseMySql(AppSettings.Configuration.GetConnectionString("MySql"), MySqlServerVersion.LatestSupportedServerVersion);
                    break;
                case "MSSQL":
                    builder.UseSqlServer(AppSettings.Configuration.GetConnectionString("MSSql"));
                    break;
            }
                
            return new BlogMigrationsDbContext(builder.Options);
        }
        
    }
}
