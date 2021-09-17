using Volo.Abp.Application.Services;
using Boo.Blog.Blog.DTO;

namespace Boo.Blog.Application.Contracts.Blog
{
    public interface IBlogService : ICrudAppService<PostDto, long>
    {

    }
}
