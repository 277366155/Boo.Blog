using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class Tag : Entity<long>
    {
        public string TagName { get; set; }
        public string DisplayName { get; set; }
    }
}
