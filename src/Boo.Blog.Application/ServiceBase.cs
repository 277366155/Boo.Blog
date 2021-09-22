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
    public class ServiceBase<TEntity, TEntityDto, TKey> : CrudAppService<TEntity, TEntityDto, TKey>
                                                                                    ,IServiceBase<TEntityDto,TKey> 
                                                                                     where TEntity : class, IEntity<TKey> where TEntityDto : IEntityDto<TKey>
    {
        public ServiceBase(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }

        //todo:封装返回类型、分页查询参数类型等
        public new async Task<ResponseDataResult<TEntityDto>> CreateAsync(TEntityDto input)
        {
            try
            {
                var data = await base.CreateAsync(input);
                return ResponseResult.IsSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail<TEntityDto>(ex);
            }
        }
        public new async Task<ResponseDataResult<TEntityDto>> GetAsync(TKey id)
        {
            try
            {
                var data = await base.GetAsync(id);
                if (data == null)
                {
                    return ResponseResult.IsFail<TEntityDto>("数据不存在");
                }
                return ResponseResult.IsSuccess(data);
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail<TEntityDto>(ex);
            }
        }

        public async Task<ResponseDataResult<PageResult<TEntityDto>>> GetListAsync(PageParam input)
        {
            try
            {
                var pageParam= ObjectMapper.Map<PageParam,PagedAndSortedResultRequestDto>(input);
                var data = await base.GetListAsync(pageParam);
                var mapData = ObjectMapper.Map<PagedResultDto<TEntityDto>, PageResult<TEntityDto>>(data);
                mapData.PageSize = input.PageSize;
                mapData.PageIndex = input.PageIndex;
                return ResponseResult.IsSuccess(mapData);
            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail<PageResult<TEntityDto>>(ex);
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
        public new async Task<ResponseDataResult<TEntityDto>> UpdateAsync(TKey id, TEntityDto input)
        {
            try
            {
                var data= await base.UpdateAsync(id, input);
                return ResponseResult.IsSuccess(data);

            }
            catch (Exception ex)
            {
                return ResponseResult.IsFail<TEntityDto>(ex);
            }
        }

    }
}
