using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;
using Volo.Abp.EntityFrameworkCore;

namespace Boo.Blog.EntityFrameworkCore.Repositories.Blog
{
    public class FriendLinkRepository : RepositoryBase<FriendLink>,IFriendLinkRepository
    {
        public FriendLinkRepository(IDbContextProvider<BlogDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
