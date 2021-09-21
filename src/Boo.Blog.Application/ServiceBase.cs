using Boo.Blog.Paged;
using Boo.Blog.Response;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Boo.Blog.Application
{
    public class ServiceBase<TEntity, TEntityDto, TKey> : CrudAppService<TEntity, TEntityDto, TKey>,IServiceBase<TEntityDto,TKey> 
                                                                                     where TEntity : class, IEntity<TKey> where TEntityDto : IEntityDto<TKey>
    {
        public ServiceBase(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }

        //todo:封装返回类型、分页查询参数类型等
        public new async Task<ResponseResult> CreateAsync(TEntityDto input)
        {
            try
            {
                var data = await base.CreateAsync(input);
                return ResponseDataResult<TEntityDto>.IsSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail(ex);
            }
        }
        public new async Task<ResponseResult> GetAsync(TKey id)
        {
            try
            {
                var data = await base.GetAsync(id);
                return ResponseDataResult<TEntityDto>.IsSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail(ex);
            }
        }

        public async Task<ResponseResult> GetListAsync(PageParam input)
        {
            try
            {
                var pageParam= ObjectMapper.Map<PageParam,PagedAndSortedResultRequestDto>(input);
                var data = await base.GetListAsync(pageParam);
                var mapData = ObjectMapper.Map<PagedResultDto<TEntityDto>, PageResult<TEntityDto>>(data);
                return ResponseDataResult<PageResult<TEntityDto>>.IsSuccess(mapData);
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail(ex);
            }
        }

        public new async Task<ResponseResult> DeleteAsync(TKey id)
        {
            try
            {
                await base.DeleteAsync(id);
                return ResponseResult.IsSucess();
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail(ex);
            }
        }
        public new async Task<ResponseResult> UpdateAsync(TKey id, TEntityDto input)
        {
            try
            {
                var data= await base.UpdateAsync(id, input);
                return ResponseDataResult<TEntityDto>.IsSuccess(data);

            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail(ex);
            }
        }

    }
}
