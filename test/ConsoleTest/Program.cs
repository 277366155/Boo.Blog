using Boo.Blog.ToolKits;
using ConsoleTest.Test;

ITest test =new TaskTest();
await test.TestStartAsync();
Console.WriteLine("main逻辑执行完毕。");
Console.Read();