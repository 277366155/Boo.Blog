using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class Post:Entity<long>
    {
        [Required]
        [StringLength(32)]
        public string Title { get; set; }
        [Required]
        [StringLength(32)]
        public string Author { get; set; }
        [Required]
        [StringLength(512)]
        public string Url { get; set; }
        [MaxLength]
        public string Html { get; set; }
        [MaxLength]
        public string Markdown { get; set; }
        [Required]
        public long CategoryId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
