using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Xunit;
using Xunit.Abstractions;

namespace Boo.Blog.TestProject
{
    public class ApplicationTest
    {
        ITestOutputHelper outPut;
        public ApplicationTest(ITestOutputHelper outPut)
        {
            this.outPut = outPut;
        }
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

            var taskList = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                taskList.Add(Task.Run(() =>
                {
                   for(var j =0;j<2000;j++)
                    {
                        Thread.Sleep(30);
                        outPut.WriteLine($"{DateTime.Now}°™°™{Thread.CurrentThread.ManagedThreadId}");
                    }
                }));
            }
            Task.WaitAll(taskList.ToArray());
        }
    }
}
