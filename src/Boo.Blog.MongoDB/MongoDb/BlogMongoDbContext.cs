using Boo.Blog.Domain.Blog;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Boo.Blog.MongoDb
{
    [ConnectionStringName("MongoDb")]
    public class BlogMongoDbContext:AbpMongoDbContext
    {
        public IMongoCollection<Post> Posts => Collection<Post>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}
