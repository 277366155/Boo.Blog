using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.Domain.MultiTenant.IRepositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore.Repositories.MultiTenant
{
    public class TenantRepository : EfCoreRepository<MultiTenantDbContext, Tenant>, ITenantRepository
    {
        public TenantRepository(IDbContextProvider<MultiTenantDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
