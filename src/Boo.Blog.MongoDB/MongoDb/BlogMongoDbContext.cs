using Boo.Blog.Domain.Blog;
using MongoDB.Driver;

namespace Boo.Blog.MongoDB
{
    public class BlogMongoDbContext: BaseMongoDbContext
    {
        public IMongoCollection<Post> Posts => Collection<Post>();

    }
}
