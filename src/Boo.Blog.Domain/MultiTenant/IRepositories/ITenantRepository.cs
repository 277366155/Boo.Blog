using Volo.Abp.Domain.Repositories;
using Volo.Abp.DependencyInjection;

namespace Boo.Blog.Domain.MultiTenant.IRepositories
{
    public interface ITenantRepository: IRepository<Tenant>,IScopedDependency
    {

    }
}
