using Abp;
using Abp.Dependency;
using System;

namespace ConsolePro
{
    class Program
    {
        static void Main(string[] args)
        {
            //通过AbpBootstrapper创建模块，并初始化
            using var bootstrapper = AbpBootstrapper.Create<ConsoleClientModule>();
            bootstrapper.Initialize();

            //通过IocManager从容器中获取对象
            var service= IocManager.Instance.Resolve<HelloService>();
            service.HelloWorld();

            Console.Read();
        }
    }
}
