﻿using AutoMapper;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.MultiTenant.DTO;
using Boo.Blog.Paged;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;

namespace Boo.Blog
{
    public class BlogAutoMapperProfile : Profile
    {
        public BlogAutoMapperProfile()
        {
            CreateMap<Post, PostDto>().ReverseMap();

            //todo: automapper泛型映射问题
            //CreateMap(typeof(PagedResultDto<>), typeof(PageResult<>))
            //    .ConvertUsing(typeof(PageConvert<>));
            CreateMap<PagedResultDto<IEntityDto>, PageResult<IEntityDto>>()
                .ForMember(d => d.PageIndex, opt => opt.Ignore())
                .ForMember(d => d.PageCount, opt => opt.Ignore())
                .ForMember(d => d.PageSize, opt => opt.Ignore())
                .ForMember(d => d.Items, opt => opt.MapFrom(s => (s.Items as IEnumerable<IEntityDto>).ToList()));

            CreateMap<Post, PostFullInfoDto>()
                .ForMember(d => d.FriendLinks, opt => opt.Ignore())
                .ForMember(d => d.CategoryInfo, opt => opt.Ignore())
                .ForMember(d => d.Tags, opt => opt.Ignore());

            CreateMap<Tenant, TenantDTO>();
        }
    }

    public class PageConvert<T> : ITypeConverter<PagedResultDto<T>, PageResult<T>>
    {
        public PageConvert()
        { }
        public PageResult<T> Convert(PagedResultDto<T> source, PageResult<T> destination, ResolutionContext context)
        {
            destination = destination ?? new PageResult<T>();
            destination.Items = (source.Items as IEnumerable<T>).ToList();
            destination.TotalCount = (int)source.TotalCount;

            return destination;
        }
    }
}
