using Volo.Abp.Application.Services;
using Boo.Blog.Application.Contracts.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;

namespace Boo.Blog.Application.Blog
{
    public class BlogService : CrudAppService<Post, PostDto, long>, IBlogService
    {
        public BlogService(IPostRepository repository) : base(repository)
        {
        }
    }
}
