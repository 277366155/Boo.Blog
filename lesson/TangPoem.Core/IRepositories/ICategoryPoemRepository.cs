using System.Collections.Generic;
using System.Threading.Tasks;
using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories;

namespace TangPoem.Core.IRepositories
{
    public interface ICategoryPoemRepository:IRepository<CategoryPoem,long>
    {
        Task<List<Category>> GetPoemCategoriesAsync(long poemId);
        Task<List<Poem>> GetPoemsOfCategoryAsync(long categoryId);
    }
}
