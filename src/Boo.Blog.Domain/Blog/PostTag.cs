using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class PostTag:Entity<long>
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        public long TagId { get; set; }            
    }
}
