using Boo.Blog.ToolKits.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp;

namespace TangPoem.EF
{
    public class PoemDbContextFactory : IDesignTimeDbContextFactory<PoemDbContext>
    {
        public PoemDbContext CreateDbContext(string[] args)
        {
            var conn = AppSettings.Root.GetConnectionString("MySql");
            var builder = new DbContextOptionsBuilder<PoemDbContext>()
                 .UseMySql(conn, ServerVersion.AutoDetect(conn));
            return new PoemDbContext(builder.Options);
        }

        //public PoemDbContext CreateDbContext(string[] args)
        //{
        //    using var app = AbpApplicationFactory.Create<PoemEFModule>();
        //    var service = app.ServiceProvider.GetService<PoemDbContext>();
        //    return service;
        //}
    }
}
