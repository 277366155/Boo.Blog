using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace TangPoem.EF.Repositories
{
    public interface IPoemRepository : IRepository<Poet, long>
    {
        Poet Insert(Poet data);
    }


    public class PoemRepository : EfCoreRepository<PoemDbContext, Poet,long>,IPoemRepository
    {
        public PoemRepository(IDbContextProvider<PoemDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Poet Insert(Poet data)
        {
            var result = DbSet.Add(data);
            return result.Entity;
        }
    }
}
