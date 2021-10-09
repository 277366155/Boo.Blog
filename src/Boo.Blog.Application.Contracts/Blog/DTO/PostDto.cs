using System;
using Volo.Abp.Application.Dtos;

namespace Boo.Blog.Blog.DTO
{
    public class PostDto : EntityDto<long>
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }
        public string Markdown { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
