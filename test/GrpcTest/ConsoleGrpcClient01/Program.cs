// The port number must match the port of the gRPC server.
using Boo.Blog.ToolKits.Configurations;
using ConsoleGrpcClient01;
using Grpc.Core;
using Grpc.Net.Client;

using var channel = GrpcChannel.ForAddress(AppSettings.Root["ServiceRemote"]);
var client = new Greeter.GreeterClient(channel);
Metadata header = new Metadata();
Console.Write("输入token：");
header.Add("token",Console.ReadLine());
var bothway = client.Bothway(header);
var cts = new CancellationTokenSource();

var backTask = Task.Factory.StartNew(async () =>
{

    try
    {
        while (await bothway.ResponseStream.MoveNext(cts.Token))
        {       
            var back = bothway.ResponseStream.Current;
            Console.WriteLine($"{back.Value}");
        }
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
    {
        Console.WriteLine("Stream cancelled. ");
    }
});

await bothway.RequestStream.WriteAsync(new StringMessage { Value = "0" });

await bothway.RequestStream.CompleteAsync();
Console.WriteLine("加载模块发送完毕");
Console.WriteLine("等待加载...");

//等待响应完成
await backTask;

Console.WriteLine("模块已全部加载完毕");

Console.ReadKey();