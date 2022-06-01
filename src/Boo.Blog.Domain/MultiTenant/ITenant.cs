using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.MultiTenant
{
    public interface ITenant : IEntity
    {
        public long Id { get; set; }
        public string TenantCode { get; set; }
        public string TenantName { get; set; }
        public long DatabaseServerId { get; set; }
        public bool IsDeleted { get; set; }
        public DatabaseServer DatabaseServer { get; set; }
    }
}
