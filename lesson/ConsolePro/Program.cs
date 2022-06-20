using Boo.Blog.ToolKits.Extensions;
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
            Init().Wait();
        }

        static async Task Init()
        {
            bool flag = true;
            while (flag)
            {
                Console.Write($"请输入选择：\r\n【s】查询所有数据。\r\n【a】插入新数据。\r\n【e】退出。\r\n请输入：");
                var input = Console.ReadKey();
                Console.WriteLine();
                //通过AbpBootstrapper创建模块，并初始化
                using var app = AbpApplicationFactory.Create<ConsoleClientModule>(opt=> opt.UseAutofac());
                app.Initialize();
                var service = app.ServiceProvider.GetService<Service>();
                switch (input.KeyChar)
                {
                    //case 'a':
                    //    await service.AddNewDataAsync();
                    //    break;
                    //case 'o':
                    //    var data= service.GetOne();
                    //    Console.WriteLine(data.ToJson());
                    //    break;
                    case 's':
                        Console.WriteLine("请输入id：");
                        var  id =Convert.ToInt64(Console.ReadLine());
                        service.GetPoemOfCategory(id);
                        break;
                    case 'p':
                        var data =service.PagedData();
                        Console.WriteLine(data.ToJson());
                        break;
                    case 'e':
                        flag = false;
                        Console.WriteLine("即将退出...");                        
                        break;
                    default:
                        Console.WriteLine("error...");
                        break;
                }
            }
        }
    }
}
