using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Boo.Blog.MongoDB.Repositories.Blog
{
    public class MongoDbPostRepository : MongoDbRepository<BlogMongoDbContext, Post, long>, IPostRepository
    {
        public MongoDbPostRepository(IMongoDbContextProvider<BlogMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public async Task<Tuple<IEnumerable<Post>, int>> GetPageListAsync(Expression<Func<Post, bool>> filter, Dictionary<string, bool> sort, int pageIndex, int pageSize, IUnitOfWork uow = null)
        {
            // var db =await GetDatabaseAsync();
            //var data = await   db.GetCollection<Post>("Posts").FindAsync<Post>(filter);
            //todo..
            throw new NotImplementedException();
        }

        public Task<Tuple<IEnumerable<T>, int>> GetPageListAsync<T>(string sql, object param, int pageIndex, int pageSize, string orderBy = null, IDbTransaction dbTransaction = null)
        {
            //todo..
            throw new NotImplementedException();
        }
    }
}
