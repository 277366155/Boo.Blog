using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore.Repositories
{
    public class RepositoryBase<TContext, TEntity> : EfCoreRepository<TContext, TEntity, long> where TEntity : class, IEntity<long> where TContext : IEfCoreDbContext
    {
        public RepositoryBase(IDbContextProvider<TContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
    public class RepositoryBase<TEntity> : EfCoreRepository<BlogDbContext, TEntity, long> where TEntity : class, IEntity<long>
    {
        public RepositoryBase(IDbContextProvider<BlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
