using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Boo.Blog.Application.HelloWorld
{
    public interface IHelloWorldService
    {
        Task<string> HelloWorld();
        bool CacheTest(string key, string value);
    }
}
