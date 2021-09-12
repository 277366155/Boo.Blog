using Boo.Blog.Application.HelloWorld;

namespace Boo.Blog.Application.HelloWorld
{
    public class HelloWorldService : BlogApplicationServiceBase, IHelloWorldService
    {
        public string HelloWorld()
        {
            return "Hello world.";
        }
    }
}