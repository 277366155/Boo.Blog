using Volo.Abp.Application.Services;
using Boo.Blog.Blog.DTO;
using System.Threading.Tasks;
using Boo.Blog.Response;

namespace Boo.Blog.Application.Contracts.Blog
{
    public interface IBlogService : IServiceBase<PostDto, long>
    {
        Task<ResponseDataResult<long>> GetPostsCountAsync();// (IEnumerable<PostDto> postDto);
    }
}
