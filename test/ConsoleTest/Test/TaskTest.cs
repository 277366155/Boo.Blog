using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Test
{
    public class TaskTest : ITest
    {
        public void TestStart()
        {
            throw new NotImplementedException();
        }

        public async Task TestStartAsync()
        {
            var count = 3;
            var taskList = new List<Task>();
            for (var i = 0; i < count; i++)
            {
                var num = i;

                var t = new Task( () =>
                {
                    for (var j = 0; j < 3; j++)
                    {
                        //Task.Delay(3000);
                        Thread.Sleep(3000);
                        Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff")}] : task i={num}, j ={j}，正在执行。");
                    }
                });
                //如果不执行t.Start()，下方的Task.WhenAll()会一直等待。
                t.Start();
                taskList.Add(t);
            }
            Console.WriteLine("TestStartAsync 执行开始。");
           await Task.WhenAny(taskList);
            //Task.WaitAll(taskList.ToArray());
            Console.WriteLine("TestStartAsync 执行完毕。");
            await Task.CompletedTask;
        }
    }
}
