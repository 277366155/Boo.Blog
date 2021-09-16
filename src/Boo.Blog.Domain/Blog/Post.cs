using System;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class Post:Entity<long>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }

        public string Markdown { get; set; }
        public long CategoryId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
