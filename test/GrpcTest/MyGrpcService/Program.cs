using MyGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

Task.Factory.StartNew(() =>
{
    app.Run();
});
Thread.Sleep(2000);

var msg = "";
var dicKey = "0";
while (!msg.ToLower().Equals("exit"))
{
    Console.Write("\r\n请输入新的消息：");
    msg= Console.ReadLine();
    
    if (msg.Contains(";"))
    { 
        dicKey=msg.Split(';')[0];
    }
    if (GreeterService.dicQueue.ContainsKey(dicKey))
    {
        GreeterService.dicQueue[dicKey] = new Queue<string>();
    }
    GreeterService.dicQueue[dicKey].Enqueue(msg.Split(';')[1]);
}
