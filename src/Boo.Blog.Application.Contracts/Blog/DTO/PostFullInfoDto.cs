using Boo.Blog.Domain.Blog;
using System.Collections.Generic;

namespace Boo.Blog.Blog.DTO
{
    public class PostFullInfoDto : PostDto
    {
        public Category CategoryInfo { get; set; }
        public List<Tag> Tags { get; set; }
        public List<FriendLink> FriendLinks { get; set; }
    }
}
