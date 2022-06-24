using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.Domain.MultiTenant.IRepositories;
using Boo.Blog.MultiTenant.DTO;
using Boo.Blog.MultiTenant.IServices;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Extensions;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Boo.Blog.Application.MultiTenant
{
    public class TenantService : ApplicationService, ITenantService
    {
        ITenantRepository _tenantRepository;
        IDatabaseServerRepository _databaseServerRepository;
        public TenantService(ITenantRepository tenantRepository, IDatabaseServerRepository databaseServerRepository)
        {
            _tenantRepository = tenantRepository;
            _databaseServerRepository = databaseServerRepository;

        }
        public async Task<TenantDTO> GetOrAddTenantByCode(string tenantCode, string tenantName)
        {
            var data = await _tenantRepository.FindAsync(a => a.TenantCode.Equals(tenantCode));
            if (data == null)
            {
                var db = await _databaseServerRepository.FindAsync(a => a.IsDefault);
                data = await _tenantRepository.InsertAsync(new Tenant { TenantCode = tenantCode, TenantName = tenantName, IsDeleted = false, DatabaseServerId = db.Id },true);
            }
            var result = ObjectMapper.Map<Tenant, TenantDTO>(data);

            return result;
        }
    }
}
