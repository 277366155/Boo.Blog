using Boo.Blog.Domain.Blog;
using Boo.Blog.MongoDB.MongoDb;
using MongoDB.Driver;

namespace Boo.Blog.MongoDb
{
    public class BlogMongoDbContext: BaseMongoDbContext
    {
        public IMongoCollection<Post> Posts => Collection<Post>();

    }
}
