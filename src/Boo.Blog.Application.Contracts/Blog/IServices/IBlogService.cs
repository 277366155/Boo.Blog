using Volo.Abp.Application.Services;
using Boo.Blog.Blog.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Boo.Blog.Application.Contracts.Blog
{
    public interface IBlogService : ICrudAppService<PostDto, long>
    {
        Task<int> BulkInsertAsync(int n);// (IEnumerable<PostDto> postDto);
    }
}
