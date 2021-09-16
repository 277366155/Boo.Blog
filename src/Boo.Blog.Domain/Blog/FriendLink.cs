using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class FriendLink : Entity<long>
    {
        public string Title { get; set; }
        public string LinkUrl { get; set; }
    }
}
