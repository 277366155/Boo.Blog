using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangPoem.Core.IRepositories;
using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace TangPoem.EF.Repositories
{
    public class CategoryPoemRepository : EfCoreRepository<PoemDbContext, CategoryPoem, long>, ICategoryPoemRepository
    {
        public CategoryPoemRepository(IDbContextProvider<PoemDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<List<Category>> GetPoemCategoriesAsync(long poemId)
        {
            var dbContext = await GetDbContextAsync();
            var cp = await dbContext.Set<CategoryPoem>()
                .Include(a => a.Category)
                .Where(a => a.PoemId.Equals(poemId))
                .Select(a => a.Category).ToListAsync();
            return cp;
        }

        public async Task<List<Poem>> GetPoemsOfCategoryAsync(long categoryId)
        {
            var dbSet = await GetDbSetAsync();
            var data =await dbSet.Include(a => a.Poem)
                .Where(a => a.CategoryId.Equals(categoryId))
                .Select(a=>a.Poem).ToListAsync() ;
            return data;
        }
    }
}
