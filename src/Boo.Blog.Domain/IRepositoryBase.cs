using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Boo.Blog.Domain
{
    public interface IRepositoryBase<TEntity>: IRepository<TEntity, long> where TEntity : class,  IEntity<long>
    {
    }
}
