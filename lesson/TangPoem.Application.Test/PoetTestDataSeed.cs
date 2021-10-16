using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangPoem.Core.Poems;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace TangPoem.Application.Test
{
    /// <summary>
    /// 插入种子数据
    /// </summary>
    public class PoetTestDataSeed : IDataSeedContributor, ITransientDependency
    {
        readonly IRepository<Poet> _poetRepositry;
        public PoetTestDataSeed(IRepository<Poet> poetRepositry)
        {
            _poetRepositry= poetRepositry;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
          await   _poetRepositry.InsertAsync(new Poet { Name="杜甫", Description="诗圣" });
          await   _poetRepositry.InsertAsync(new Poet { Name = "李白", Description = "诗仙" });
        }
    }
}
