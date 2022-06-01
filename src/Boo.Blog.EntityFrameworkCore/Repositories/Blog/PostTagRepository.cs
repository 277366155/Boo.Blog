using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore.Repositories.Blog
{
    public class PostTagRepository : RepositoryBase<PostTag>, IPostTagRepository
    {
        public PostTagRepository(IDbContextProvider<BlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
