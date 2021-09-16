using Volo.Abp.Application.Services;

namespace Boo.Blog.Application.HelloWorld
{
    public class HelloWorldService : ApplicationService, IHelloWorldService
    {
        public string  HelloWorld()
        {
            return "hello world.";
        }
    }
}
