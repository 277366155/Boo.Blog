using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;
using Boo.Blog.MongoDb;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Boo.Blog.MongoDB.Repositories.Blog
{
    public class MongoDbPostRepository : MongoDbRepository<BlogMongoDbContext, Post, long>, IPostRepository
    {
        public MongoDbPostRepository(IMongoDbContextProvider<BlogMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }
    }
}
