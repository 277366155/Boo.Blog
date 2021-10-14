using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace ConsolePro
{
    class Program
    {
        static void Main(string[] args)
        {
             Init();
            Console.Read();
        }

        static async void Init()
        {
            bool flag = true;
            while (flag)
            {   
                Console.Write($"请输入选择：\r\n【s】查询所有数据。\r\n【a】插入新数据。\r\n【e】退出。\r\n请输入：");
                var input = Console.ReadKey();
                //通过AbpBootstrapper创建模块，并初始化
                using var app = AbpApplicationFactory.Create<ConsoleClientModule>();
                app.Initialize();
                var service = app.ServiceProvider.GetService<Service>();
                switch (input.KeyChar)
                {
                    case 's':
                        service?.GetFirstPoetName();
                        break;
                    case 'a':
                         await service.AddNewDataAsync();
                        break;
                    case 'e':
                        flag = false;
                        Console.WriteLine("即将退出");                        
                        break;
                     default:
                        Console.WriteLine("error");
                        break;
                }
            }
        }
    }
}
