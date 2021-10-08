using Boo.Blog.ToolKits.Cache;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Boo.Blog.ToolKits.Interceptor;
using Volo.Abp.Application.Services;

namespace Boo.Blog.Application.HelloWorld
{
    public class HelloWorldService : ApplicationService, IHelloWorldService
    {

        public async Task<string> HelloWorld()
        {
            return await  RedisHandler.TryToGetFromCacheAsync("helloworldkey", 60 * 10, () =>{return "hello world."; });
        }
        public bool CacheTest(string key, string value)
        {
            return RedisHelper.Set(key,value);
        }
    }
}
