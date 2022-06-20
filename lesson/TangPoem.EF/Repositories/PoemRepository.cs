using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangPoem.Core.IRepositories;
using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace TangPoem.EF.Repositories
{
    public class PoemRepository : EfCoreRepository<PoemDbContext, Poem, long>, IPoemRepository
    {
        public PoemRepository(IDbContextProvider<PoemDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<(List<Poem>,int)> GetPagedPoemsAsync(int maxResult, int skip, string author, string keyword, string[] categories)
        {
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentException($"'{nameof(author)}' cannot be null or empty.", nameof(author));
            }

            if (string.IsNullOrEmpty(keyword))
            {
                throw new ArgumentException($"'{nameof(keyword)}' cannot be null or empty.", nameof(keyword));
            }

            if (categories is null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            var set = (await GetDbSetAsync()).Include(a => a.Author).Include(a => a.PoemCategories).AsQueryable();
            if (!author.IsNullOrWhiteSpace())
            {
                set = set.Where(a=>a.Author.Name.Equals(author));
            }
            if (!keyword.IsNullOrWhiteSpace())
            {
                set = set.Where(a=>a.Title.Contains(keyword)||a.Content.Contains(keyword));
            }
            if (categories != null && categories.Count() > 0)
            {
                foreach (var category in categories)
                {
                    set = set.Where(a => a.PoemCategories.Any(s => s.Category.Equals(category)));
                }
            }
            var total = set.Count();
            var result = await  set.OrderBy(a => a.Id).PageBy(skip, maxResult).ToListAsync();
            return (result,total);
        }
    }
}
