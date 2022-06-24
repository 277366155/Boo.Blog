using Boo.Blog.Paged;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Boo.Blog
{
    public interface IServiceBase<TEntityDto, in TKey> : ICrudAppService<TEntityDto, TKey>
    {
        //public new Task<TEntityDto> CreateAsync(TEntityDto input);

        //public new Task<TEntityDto> GetAsync(TKey id);

        //public new Task DeleteAsync(TKey id);

        //public new Task<TEntityDto> UpdateAsync(TKey id, TEntityDto input);
    }

    public interface IServiceBase<TEntity,TEntityDto, in TKey> : IServiceBase<TEntityDto, TKey>
    {
        public Task<PageResult<TEntityDto>> GetListAsync(PageParam<TEntity> input);
    }
}
