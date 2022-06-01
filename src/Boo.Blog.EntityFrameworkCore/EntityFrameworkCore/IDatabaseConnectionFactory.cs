using System.Threading.Tasks;

namespace Boo.Blog.EntityFrameworkCore
{
    public interface IDatabaseConnectionFactory
    {
        string GetConnectionString(long tenantId);
    }
}
