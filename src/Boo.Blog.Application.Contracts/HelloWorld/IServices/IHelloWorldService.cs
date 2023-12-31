﻿using System.Threading.Tasks;

namespace Boo.Blog.HelloWorld
{
    public interface IHelloWorldService
    {
        Task<string> HelloWorld();
        bool CacheTest(string key, string value);
    }
}
