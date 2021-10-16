using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace TangPoem.Application.Poems
{
    public interface IPoemApplicationService:IApplicationService
    {
        PagedResultDto<PoetDto> GetPagedPoets(PagedResultRequestDto paged);
        //PagedResultDto<PoetDto> SearchPoets(SearchPoetDto param);
        PagedResultDto<PoemDto> GetPagedPoems(PagedResultRequestDto param);
        //PagedResultDto<PoemDto>  SearchPoems(SearchPoemsDto param);
        Task<CategoryDto> AddCategoryAsync(CategoryDto category);
        void DeleteCategory(CategoryDto category);
        List<CategoryDto> GetAllCategories();
        Task AddPoemToCategoryAsync(CategoryPoemDto categoryPoem);
        void RemovePoemFromCategory(CategoryPoemDto categoryPoem);
        List<CategoryPoemDto> GetAllCategoryPoems();
        List<CategoryDto> GetCategories(long poemId);
        List<PoemDto> GetPoemOfCategory(long categoryId);
        Task<PoetDto> AddPoetAsync(PoetDto poet);
    }
}
