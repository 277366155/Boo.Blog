using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.Domain.MultiTenant.IRepositories;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore.Repositories.MultiTenant
{
    public class DatabaseServerRepository : RepositoryBase<MultiTenantDbContext, DatabaseServer>,IDatabaseServerRepository
    {
        public DatabaseServerRepository(IDbContextProvider<MultiTenantDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
