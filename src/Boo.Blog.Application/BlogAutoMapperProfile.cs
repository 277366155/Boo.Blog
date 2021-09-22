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
            CreateMap<PageParam, PagedAndSortedResultRequestDto>()
                .ForMember(d => d.SkipCount, opt => opt.MapFrom(a=> a.PageIndex > 0 ? a.PageIndex * a.PageSize : 0))
                .ForMember(d=>d.MaxResultCount, opt => opt.MapFrom(a=>a.PageSize))
                .ForMember(d=>d.Sorting,opt=>opt.MapFrom(a=>a.Sort))
                .ReverseMap();

            //todo: automapper泛型映射问题
            CreateMap<PagedResultDto<object>, PageResult<object>>()
                .ForMember(d => d.PageIndex, opt => opt.Ignore())
                .ForMember(d => d.PageCount, opt => opt.Ignore())
                .ForMember(d => d.PageSize, opt => opt.Ignore());
                
        }
    }
}
