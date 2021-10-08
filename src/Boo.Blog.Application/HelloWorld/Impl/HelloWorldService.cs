using Autofac.Extras.DynamicProxy;
using Boo.Blog.ToolKits.Interceptor;
using Volo.Abp.Application.Services;

namespace Boo.Blog.Application.HelloWorld
{
    public class HelloWorldService : ApplicationService, IHelloWorldService
    {

        public string  HelloWorld()
        {
            return "hello world.";
        }
        public bool CacheTest(string key, string value)
        {
            return RedisHelper.Set(key,value);
        }
    }
}
