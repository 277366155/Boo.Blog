using Boo.Blog.Paged;
using Boo.Blog.Response;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Boo.Blog
{
    public interface IServiceBase<TEntityDto, in TKey> : ICrudAppService<TEntityDto, TKey>
    {
        public new Task<ResponseResult> CreateAsync(TEntityDto input);

        public new Task<ResponseResult> GetAsync(TKey id);

        public Task<ResponseResult> GetListAsync(PageParam input);

        public new Task<ResponseResult> DeleteAsync(TKey id);

        public new Task<ResponseResult> UpdateAsync(TKey id, TEntityDto input);

    }
}
