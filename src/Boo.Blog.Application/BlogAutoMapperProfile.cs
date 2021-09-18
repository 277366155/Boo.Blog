using AutoMapper;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;

namespace Boo.Blog
{
    public class BlogAutoMapperProfile : Profile
    {
        public BlogAutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}
