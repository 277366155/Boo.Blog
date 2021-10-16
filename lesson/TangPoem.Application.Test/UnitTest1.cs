using Boo.Blog.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangPoem.Application.Poems;
using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TangPoem.Application.Test
{
    public class UnitTest1:PoemTestBase<PoemApplicationTestModule>
    {
        readonly IRepository<Poet, long> poetRepository;
        readonly IPoemApplicationService poemApplicationService;
        ITestOutputHelper output;
        public UnitTest1(ITestOutputHelper output)
        {
            poetRepository = GetRequiredService<IRepository<Poet, long>>();
            poemApplicationService= GetRequiredService<IPoemApplicationService>();
            this.output = output;
        }

        [Fact]
        public async Task TestAddPoet()
        {
            await WithUnitOfWorkAsync(async ()=>{
                var poet = new Poet { Name = "李贺", Description = "诗鬼a" };
                var addedPoet= await poetRepository.InsertAsync(poet,true);
                Assert.True(poetRepository.Count()>1);
                output.WriteLine(addedPoet.ToJson());
            });
        }
    }
}
