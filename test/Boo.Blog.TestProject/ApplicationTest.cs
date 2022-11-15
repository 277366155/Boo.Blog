using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;
using Boo.Blog.ToolKits;
using System.Diagnostics;
using Newtonsoft.Json;
using Boo.Blog.ToolKits.Extensions;

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
            //    var data =await blogService.CreateAsync(new PostDto() { Title="测试一下", Author="boo" });
            //    var getData = await blogService.GetAsync(data.Data.Id);
            //    Assert.NotNull(getData.Data);
            //}

            var taskList = new List<Task>();
            for (var i = 0; i < 10; i++)
            {
                taskList.Add(Task.Run(() =>
                {
                    for (var j = 0; j < 2000; j++)
                    {
                        Thread.Sleep(30);
                        outPut.WriteLine($"{DateTime.Now}——{Thread.CurrentThread.ManagedThreadId}");
                    }
                }));
            }
            Task.WaitAll(taskList.ToArray());
        }

        [Theory]
        [InlineData("testkey1", "testValue", 200)]
        [InlineData("testkey1", "testValue2", 200)]
        public void RedisLockTest(string key, string value, int timespan)
        {
            var redisHandler = new RedisHandler(AppSettings.Root.GetSection("Redis").Get<IEnumerable<RedisHandlerOption>>());
            var redisClient = redisHandler.GetRedisClient(RedisType.Default);
            outPut.WriteLine(redisClient.Set(key, value, timespan, CSRedis.RedisExistence.Nx).ToString());
        }

        [Theory]
        [InlineData(1000,"1qaz@WSX3edc$RFV")]
        [InlineData(10000, "1qaz@WSX3edc$RFV")]
        [InlineData(100000, "1qaz@WSX3edc$RFV")]
        [InlineData(1000, "广东省深圳市南山区粤海街道深南大道104号软件产业基地2C栋7楼办事处")]
        [InlineData(10000, "广东省深圳市南山区粤海街道深南大道104号软件产业基地2C栋7楼办事处")]
        [InlineData(100000, "广东省深圳市南山区粤海街道深南大道104号软件产业基地2C栋7楼办事处")]
        public void SM4Test(int count,string msg)
        {
            var result = SM4Util.Sm4EncryptECB(msg);
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < count; i++)
            {
                SM4Util.Sm4EncryptECB(msg);
            }
            sw.Stop();
            outPut.WriteLine($"[ {msg} ]长度为：{msg.Length}，加密 {count} 次，耗时[{sw.ElapsedMilliseconds}]ms");
            sw.Restart();
            for (var i = 0; i < count; i++)
            {
                SM4Util.Sm4DecryptECB(result);
            }
            sw.Stop();
            outPut.WriteLine($"解密 {count} 次，耗时[ {sw.ElapsedMilliseconds} ]ms");
        }

        [Fact]
        public void TestJsonConvert()
        {
            dynamic s = new { data =true,b="test" };
            outPut.WriteLine(s.data.ToString());
            var res=JsonConvert.DeserializeObject<bool>(s.data.ToString().ToLower());
            outPut.WriteLine(res.ToString());
        }

        [Fact]
        public void GenerateSerialNumberTest()
        {
            var r=  Helper.GenerateDetail();
            outPut.WriteLine(r.ToJson());
        }
    }
}
