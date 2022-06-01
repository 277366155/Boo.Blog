using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.MultiTenant
{
    public class DatabaseServer:Entity<long>
    {
       [StringLength(128)]
        public string Host { get; set; }
        [StringLength(128)]
        public string DbName { get; set;}
        [StringLength(128)]
        public string UserId { get; set;}
        [StringLength(128)]
        public string DbPassword { get; set;}

    }
}
