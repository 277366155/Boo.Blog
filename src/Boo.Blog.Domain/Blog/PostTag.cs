using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class PostTag:Entity<long>
    {
        public long PostId { get; set; }
        public long TagId { get; set; }            
    }
}
