using Boo.Blog.ToolKits.Configurations;
using CSRedis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyGrpcService.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions());

// Add services to the container.
builder.Services.AddGrpc();
var app = builder.Build();
RedisHelper.Initialization(new CSRedisClient(AppSettings.Root["RedisConnect"]));

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<UserService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
//Task.Factory.StartNew(() =>
//{
//    app.Run();
//});
Thread.Sleep(2000);

var msg = "";
var token = "";
while (!msg.ToLower().Equals("exit"))
{
    Console.Write("\r\n请输入新的消息：");
    msg = Console.ReadLine();
    var queueMsg = string.Empty;
    if (msg.Contains(";") && !string.IsNullOrWhiteSpace(msg.Split(';')[0]))
    {
        token = msg.Split(';')[0];
        queueMsg = msg.Split(';')[1];
        UserServiceQueue(token, queueMsg);
    }
}


static void UserServiceQueue(string token, string queueMsg)
{
    RedisHelper.StartPipe().RPush(token, queueMsg).Expire(token, 60 * 60 * 24 * 3).EndPipe();
}

static void GreeterServiceDic(string dicKey, string queueMsg)
{
    if (GreeterService.dicQueue.ContainsKey(dicKey))
    {
        if (GreeterService.dicQueue[dicKey] == null)
        {
            GreeterService.dicQueue[dicKey] = new Queue<string>();
        }
        GreeterService.dicQueue[dicKey].Enqueue(queueMsg);
    }
}