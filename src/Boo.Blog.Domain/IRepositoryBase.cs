using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace Boo.Blog.Domain
{
    public interface IRepositoryBase<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity<long>
    {
        Task<Tuple<IEnumerable<TEntity>, int>> GetPageListAsync(Expression<Func<TEntity, bool>> filter, Dictionary<string, bool> sort, int pageIndex, int pageSize, IUnitOfWork uow = null);
        Task<Tuple<IEnumerable<T>, int>> GetPageListAsync<T>(string sql, object param, int pageIndex, int pageSize, string orderBy = null, IDbTransaction dbTransaction = null);
    }
}
