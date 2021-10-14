using Microsoft.EntityFrameworkCore;
using TangPoem.Core.Poems;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace TangPoem.EF
{
    //[ConnectionStringName("MySql")]
    public class PoemDbContext : AbpDbContext<PoemDbContext>
    {
        public  DbSet<Poet> Poets { get; set; }
        //public PoemDbContext(DbContextOptions options) : base(options)
        //{

        //}
        public PoemDbContext(DbContextOptions<PoemDbContext> options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //}
    }

    public class ConnectionStringOptions
    {
        public string ConnectionString { get; set; }
    }
}
