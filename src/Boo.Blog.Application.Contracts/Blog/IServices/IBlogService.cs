using Volo.Abp.Application.Services;
using Boo.Blog.Blog.DTO;
using System.Threading.Tasks;

namespace Boo.Blog.Application.Contracts.Blog
{
    public interface IBlogService : ICrudAppService<PostDto, long>
    {
        Task<long> GetPostsCountAsync();// (IEnumerable<PostDto> postDto);
    }
}
