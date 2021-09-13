using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.Application.HelloWorld.Impl
{
    public class HelloWorldService : IHelloWorldService
    {
        public string  HelloWorld()
        {
            return "hello world.";
        }
    }
}
