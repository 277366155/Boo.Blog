// The port number must match the port of the gRPC server.
using Boo.Blog.ToolKits.Configurations;
using ConsoleGrpcClient01;

//await BothwayStreamClient.InitConnectAsync();
Console.WriteLine("输入guid串：");
string guid = AppSettings.Root["GrpcClient:ClientGuid"];//  Guid.NewGuid().ToString("N").Substring(0, 4);
Console.WriteLine("输入grpc客户端数量：");
int count =Convert.ToInt32(AppSettings.Root["GrpcClient:ClientCount"]);// int.Parse(Console.ReadLine());
ServerStreamClient serverStreamClient = new ServerStreamClient();
List<Task> taskList = new List<Task>();
for (var i = 0; i < count; i++)
{
    var token = $"grpcToken_{guid}_{i}";
    taskList.Add( serverStreamClient.InitConnectAsync(token));
    Thread.Sleep(100);
}
Task.WaitAll(taskList.ToArray());
Console.ReadKey();

