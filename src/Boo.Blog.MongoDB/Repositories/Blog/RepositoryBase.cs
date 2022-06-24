using Boo.Blog.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Boo.Blog.MongoDB.Repositories.Blog
{
    public class RepositoryBase<TEntity> : MongoDbRepository<BlogMongoDbContext, TEntity, long>, IRepositoryBase<TEntity> where TEntity:class,IEntity<long>
    {
        public RepositoryBase(IMongoDbContextProvider<BlogMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<Tuple<IEnumerable<TEntity>, int>> GetPageListAsync(FilterDefinition<Func<TEntity, bool>> filter, Dictionary<Expression<Func<TEntity, object>>, bool> sort, int pageIndex, int pageSize, IUnitOfWork uow = null)
        {
            var dbContext=await GetDbContextAsync();
            var data =await  dbContext.Collection<TEntity>().FindAsync<TEntity>();
            throw new NotImplementedException();
        }

        public Task<Tuple<IEnumerable<T>, int>> GetPageListAsync<T>(string sql, object param, int pageIndex, int pageSize, string orderBy = null, IDbTransaction dbTransaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
