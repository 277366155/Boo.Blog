using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Boo.Blog.MongoDB.MongoDb
{
    [ConnectionStringName("MongoDb")]
    public class BaseMongoDbContext : AbpMongoDbContext
    {
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}
