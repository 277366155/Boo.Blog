using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.MultiTenant.DTO;
using System.Threading.Tasks;

namespace Boo.Blog.MultiTenant.IServices
{
    public interface ITenantService
    {
        Task<TenantDTO> GetOrAddTenantByCode(string tenantCode,string tenantName);
    }
}
