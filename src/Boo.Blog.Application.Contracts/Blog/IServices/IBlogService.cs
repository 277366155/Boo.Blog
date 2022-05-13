using Volo.Abp.Application.Services;
using Boo.Blog.Blog.DTO;
using System.Threading.Tasks;
using Boo.Blog.Response;
using Boo.Blog.Domain.Blog;

namespace Boo.Blog.Blog
{
    public interface IBlogService : IServiceBase<Post,PostDto, long>
    {
        Task<ResponseDataResult<long>> GetPostsCountAsync();// (IEnumerable<PostDto> postDto);
        Task<ResponseDataResult<PostFullInfoDto>> GetPostFullInfoAsync(long id);
        Task DeletePostAsync(int id);
    }
}
