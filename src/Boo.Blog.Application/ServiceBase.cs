using Boo.Blog.Paged;
using Boo.Blog.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace Boo.Blog.Application
{
    public class ServiceBase<TEntity, TEntityDto, TKey> : CrudAppService<TEntity, TEntityDto, TKey>
                                                                                    , IServiceBase<TEntity, TEntityDto, TKey>
                                                                                     where TEntity : class, IEntity<TKey> where TEntityDto : IEntityDto<TKey>
    {
        public ServiceBase(IRepository<TEntity, TKey> repository) : base(repository)
        {
        }

        //todo:封装返回类型、分页查询参数类型等
        public new async Task<TEntityDto> CreateAsync(TEntityDto input)
        {
            var data = await base.CreateAsync(input);
            return data;
        }
        public new async Task<TEntityDto> GetAsync(TKey id)
        {
            var data = await base.GetAsync(id);
            if (data == null)
            {
                throw new Exception("数据不存在");
            }
            return data;
        }

        public async Task<PageResult<TEntityDto>> GetListAsync(PageParam<TEntity> input)
        {
            var entities = await Repository.GetQueryableAsync();
            var iqueryable = entities.Where(a => true);// (input.Filters);
            IOrderedEnumerable<TEntity> iOrderedEnumerable = null;
            //if (input.Sorts!=null)
            //{
            //    foreach (var d in input.Sorts)
            //    {
            //        var pType = typeof(TEntity).GetProperty(d.Key).PropertyType;
            //        //Func<TEntity, dynamic> func = ExpressionParser.Compile(" m => m.GetType().GetProperty(d.Key)");

            //        //todo:将字典转为lambda表达式。参考dapper.extension源码
            //        //if (d.Value)
            //        //{
            //        //    iOrderedEnumerable = iqueryable.OrderBy(a =>a.GetType().GetProperty(d.Key));
            //        //}
            //        //else
            //        //{
            //        //    iOrderedEnumerable = iqueryable.OrderByDescending(a => d.Key);
            //        //}
            //    }
            //}
            var resultData = iqueryable.Skip(input.PageIndex * input.PageSize).Take(input.PageSize).ToList();

            return new PageResult<TEntityDto>
            {
                PageSize = input.PageSize,
                PageIndex = input.PageIndex,
                Items = ObjectMapper.Map<List<TEntity>, List<TEntityDto>>(resultData),
                TotalCount = iqueryable.Count()
            };

        }

        public new async Task DeleteAsync(TKey id)
        {
            await base.DeleteAsync(id);
        }
        public new async Task<TEntityDto> UpdateAsync(TKey id, TEntityDto input)
        {
            var data = await base.UpdateAsync(id, input);
            return data;
        }
    }
}
