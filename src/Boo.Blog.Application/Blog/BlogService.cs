using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;
using Boo.Blog.Response;

namespace Boo.Blog.Application.Blog
{
    public class BlogService : ServiceBase<Post, PostDto, long>, IBlogService 
    {
        public BlogService(IRepository<Post, long> repository) : base(repository)
        {
        }
        public async Task<ResponseDataResult<long>> GetPostsCountAsync()
        {
            var data = await Repository.GetCountAsync();
            return ResponseResult.IsSuccess(data, "ok");
        }
    }
}
