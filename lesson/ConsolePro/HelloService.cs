using Abp.Dependency;
using System;

namespace ConsolePro
{
    public class HelloService:ITransientDependency
    {
        public void HelloWorld()
        {
            Console.WriteLine("Hello world.");
        }
    }
}
