using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore.Repositories.Blog
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IDbContextProvider<BlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
