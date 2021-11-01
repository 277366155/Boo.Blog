using AutoMapper;
using TangPoem.Application.Poems;
using TangPoem.Core.Poems;

namespace TangPoem.Application
{
    public  class PoemApplicationAutoMapperProfile:Profile
    {
        public PoemApplicationAutoMapperProfile()
        {
            AutoMapperConfig();
        }

        void AutoMapperConfig()
        {
            CreateMap<Poet,PoetDto>().ReverseMap();
            CreateMap<Poem,PoemDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<CategoryPoem,CategoryPoemDto>().ReverseMap();
        }
    }
}
