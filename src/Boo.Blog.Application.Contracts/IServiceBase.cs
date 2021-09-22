using Boo.Blog.Paged;
using Boo.Blog.Response;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Boo.Blog
{
    public interface IServiceBase<TEntityDto, in TKey> : ICrudAppService<TEntityDto, TKey>
    {
        public new Task<ResponseDataResult<TEntityDto>> CreateAsync(TEntityDto input);

        public new Task<ResponseDataResult<TEntityDto>> GetAsync(TKey id);

        public Task<ResponseDataResult<PageResult<TEntityDto>>> GetListAsync(PageParam input);

        public new Task<ResponseResult> DeleteAsync(TKey id);

        public new Task<ResponseDataResult<TEntityDto>> UpdateAsync(TKey id, TEntityDto input);

    }
}
