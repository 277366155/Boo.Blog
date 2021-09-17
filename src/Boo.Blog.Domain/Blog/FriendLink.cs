using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class FriendLink : Entity<long>
    {
        [Required]
        [StringLength(32)]
        public string Title { get; set; }
        [Required]
        [StringLength(512)]
        public string LinkUrl { get; set; }
    }
}
