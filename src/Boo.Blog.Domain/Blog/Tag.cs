using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class Tag : Entity<long>
    {
        [Required]
        [StringLength(32)]
        public string TagName { get; set; }
        [Required]
        [StringLength(32)]
        public string DisplayName { get; set; }
    }
}
