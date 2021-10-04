using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.Application.HelloWorld
{
    public interface IHelloWorldService
    {
        string HelloWorld();
        bool CacheTest(string key, string value);
    }
}
