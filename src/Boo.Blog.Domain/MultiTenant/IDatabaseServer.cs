using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.MultiTenant
{
    public interface IDatabaseServer:IEntity<long>
    {
        public string Host { get; set; }
        public string DbName { get; set; }
        public string UserId { get; set; }
        public string DbPassword { get; set; }
        public ICollection<ITenant> Tenants { get; set; }
    }
}
