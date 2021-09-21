using AutoMapper;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using Boo.Blog.Paged;
using Volo.Abp.Application.Dtos;

namespace Boo.Blog
{
    public class BlogAutoMapperProfile : Profile
    {
        public BlogAutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<PageParam, PagedAndSortedResultRequestDto>().AfterMap((s, d) => {
                d.SkipCount = s.PageIndex > 0 ? s.PageIndex * s.PageSize : 0;
                d.MaxResultCount = s.PageSize;
                d.Sorting = s.Sort;
            });
        }
    }
}
