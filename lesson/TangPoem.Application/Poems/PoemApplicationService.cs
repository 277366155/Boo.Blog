using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangPoem.Application.Poems.Dto;
using TangPoem.Core.IRepositories;
using TangPoem.Core.Poems;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace TangPoem.Application.Poems
{
    public class PoemApplicationService : ApplicationService, IPoemApplicationService
    {
        IRepository<Poet> _poetRepository;
        IPoemRepository _poemRepository;
        IRepository<Category> _categoryRepository;
        ICategoryPoemRepository _categoryPoemRepository;
        public PoemApplicationService(IRepository<Poet> poetRepository
            , IPoemRepository poemRepository
            , IRepository<Category> categoryRepository
            , ICategoryPoemRepository categoryPoemRepository)
        {
            _poetRepository = poetRepository;
            _poemRepository = poemRepository;
            _categoryRepository = categoryRepository;
            _categoryPoemRepository = categoryPoemRepository;
        }

        public async Task<CategoryDto> AddCategoryAsync(CategoryDto category)
        {
            if (_categoryRepository.Any(a => a.Name == category.Name))
            {
                return ObjectMapper.Map<Category, CategoryDto>(_categoryRepository.FirstOrDefault(a => a.Name.Equals(category.Name)));
            }
            else
            {
                return ObjectMapper.Map<Category, CategoryDto>(await _categoryRepository.InsertAsync(ObjectMapper.Map<CategoryDto, Category>(category)));
            }
        }

        public async Task AddPoemToCategoryAsync(CategoryPoemDto categoryPoem)
        {
            await _categoryPoemRepository.InsertAsync(ObjectMapper.Map<CategoryPoemDto, CategoryPoem>(categoryPoem));
        }

        public async Task<PoetDto> AddPoetAsync(PoetDto poet)
        {
            if (_poetRepository.Any(a => a.Name == poet.Name))
            {
                return ObjectMapper.Map<Poet, PoetDto>(_poetRepository.FirstOrDefault(a => a.Name == poet.Name));
            }
            else
            {
                var data = await _poetRepository.InsertAsync(ObjectMapper.Map<PoetDto, Poet>(poet));
                return ObjectMapper.Map<Poet, PoetDto>(data);
            }
        }

        public void DeleteCategory(CategoryDto category)
        {
            _categoryRepository.DeleteAsync(a => a.Id.Equals(category.Id)).Wait();
        }

        public List<CategoryDto> GetAllCategories()
        {
            var data = _categoryRepository.ToList();
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(data);
        }

        public List<CategoryPoemDto> GetAllCategoryPoems()
        {
            var data = _categoryPoemRepository.ToList();
            return ObjectMapper.Map<List<CategoryPoem>, List<CategoryPoemDto>>(data);
        }

        public List<CategoryDto> GetCategories(long poemId)
        {
            var data = _categoryPoemRepository.Where(a => a.PoemId.Equals(poemId)).ToList();
            var result = _categoryRepository.Where(a => data.Any(b => b.CategoryId.Equals(a.Id))).ToList();
            return ObjectMapper.Map<List<Category>, List<CategoryDto>>(result);
        }

        public PagedResultDto<PoemDto> GetPagedPoems(PagedResultRequestDto param)
        {
            var data = _poemRepository.OrderBy(a => a.Id).PageBy(param).ToList();
            var count = _poemRepository.Count();
            return new PagedResultDto<PoemDto> { Items = ObjectMapper.Map<List<Poem>, List<PoemDto>>(data), TotalCount = count };
        }

        public PagedResultDto<PoetDto> GetPagedPoets(PagedResultRequestDto paged)
        {
            var count = _poetRepository.Count();
            var data = _poetRepository.OrderBy(a => a.Id).PageBy(paged).ToList();

            return new PagedResultDto<PoetDto>()
            {
                Items = ObjectMapper.Map<List<Poet>, List<PoetDto>>(data),
                TotalCount = count
            };
        }

        public List<PoemDto> GetPoemOfCategory(long categoryId)
        {
            var poemIds = _categoryPoemRepository.Where(a => a.CategoryId.Equals(categoryId))?.Select(a => a.PoemId).ToList();
            var data = _poemRepository.Where(a => poemIds.Contains(a.Id)).ToList();
            return ObjectMapper.Map<List<Poem>, List<PoemDto>>(data);
        }

        public void RemovePoemFromCategory(CategoryPoemDto categoryPoem)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<PoemDto>> SearchPoemsAsync(SearchPoemDto param)
        {
            var data = await _poemRepository.GetPagedPoemsAsync(param.MaxResultCount, param.Skip, param.Author, param.Keyword, param.Categories);
            return new PagedResultDto<PoemDto>()
            {
                TotalCount = data.Item2,
                Items = ObjectMapper.Map<List<Poem>, List<PoemDto>>(data.Item1)
            };
        }
    }

}
