using Boo.Blog.Application.Contracts.Blog;
using Boo.Blog.Blog.DTO;
using System.Threading.Tasks;
using Volo.Abp;
using Xunit;

namespace Boo.Blog.TestProject
{
    public class ApplicationTest
    {
        [Fact]
        public void Test1()
        {
            //using (var application = AbpApplicationFactory.Create<BlogTestModule>(options => {
            //    options.UseAutofac();
            //    }))
            //{
            //    var blogService=  (IBlogService)application.ServiceProvider.GetService(typeof(IBlogService));
            //    var data =await blogService.CreateAsync(new PostDto() { Title="≤‚ ‘“ªœ¬", Author="boo" });
            //    var getData = await blogService.GetAsync(data.Data.Id);
            //    Assert.NotNull(getData.Data);
            //}
        }
    }
}
