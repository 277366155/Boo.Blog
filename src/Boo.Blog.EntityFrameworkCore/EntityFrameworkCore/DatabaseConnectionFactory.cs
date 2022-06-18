using Boo.Blog.Domain.MultiTenant.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Boo.Blog.EntityFrameworkCore
{
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory,IScopedDependency
    {
        ConcurrentDictionary<long, string> _connectionStringDic = new ConcurrentDictionary<long, string>();
        readonly ITenantRepository _tenantRepository;
        readonly IDatabaseServerRepository _databaseServers;
        static int maxPoolSize = 50;
        static int minPoolSize = 10;
        static int connectTimeout = 600;
        static string applicationName = "blog";
        public DatabaseConnectionFactory(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        public string GetConnectionString(long tenantId)
        {
           return  _connectionStringDic.GetOrAdd(tenantId, id =>
            {
                var tenant = _tenantRepository.AsQueryable().Include(a => a.DatabaseServer).FirstOrDefault(a => a.Id.Equals(id));
                if (tenant == null || tenant.DatabaseServer == null)
                    return null;
                //var connectionString = $@"Data Source={ tenant?.DatabaseServer?.Host};Initial Catalog={ tenant?.DatabaseServer?.DbName };
                //                    User ID={ tenant?.DatabaseServer?.UserId };Password={ tenant?.DatabaseServer?.DbPassword };
                //                    Max Pool Size={maxPoolSize};Min Pool Size={minPoolSize};Connect Timeout={connectTimeout};
                //                    Pooling=true;MultipleActiveResultSets=true;Application Name={applicationName};";
                var connectionString = $@"Server={ tenant?.DatabaseServer?.Host};user id ={ tenant?.DatabaseServer?.UserId };password={ tenant?.DatabaseServer?.DbPassword };database={ tenant?.DatabaseServer?.DbName };";
                return connectionString;
            });
        }
    }
}
