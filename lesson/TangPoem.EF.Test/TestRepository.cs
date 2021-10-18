using Boo.Blog.ToolKits.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangPoem.Core.Poems;
using Volo.Abp.Domain.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace TangPoem.EF.Test
{
   public  class TestRepository:PoemEFTestBase
    {
        IRepository<Category> categoryRepository;
        ITestOutputHelper output;
        public TestRepository(ITestOutputHelper output)
        {
            this.output = output;
            categoryRepository =GetRequiredService<IRepository<Category>>();
        }

        [Fact]
        public async Task TestAddCategory()
        { 
            await WithUnitOfWorkAsync(async ()=> {
                var category =await  categoryRepository.InsertAsync(new Category {  Name="测试"},true);
                output.WriteLine(category.ToJson());
                Assert.True(category.Id>0);
            });
        }
    }
}
