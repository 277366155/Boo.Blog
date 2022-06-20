using Boo.Blog.Domain.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore
{
    [ConnectionStringName("MySql")]
    public class MultiTenantDbContext : AbpDbContext<MultiTenantDbContext>
    {
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<DatabaseServer> DatabaseServers { get; set;}
        public MultiTenantDbContext(DbContextOptions<MultiTenantDbContext> options) : base(options)
        {

        }
    }
}
