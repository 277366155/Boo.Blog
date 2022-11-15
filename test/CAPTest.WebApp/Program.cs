using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Enums;
using Microsoft.Extensions.Configuration;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration(builder => {
    var configRoot = builder.Build();
    builder.AddApollo(configRoot.GetSection("apollo"))
    .AddDefault()
    .AddNamespace("appsettings", ConfigFileFormat.Json);
});
builder.Services.AddControllers();
builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
builder.Services.AddCap(a => {
    //a.UseInMemoryStorage();
    //a.UseInMemoryMessageQueue();
    //a.UseRedis(builder.Configuration["Redis"]);
    //a.UseMySql(builder.Configuration["MySql"]);
    a.UseSqlServer(config["SqlServer"]);
    a.UseRabbitMQ(opt=> {
        opt.UserName = config["RabbitMQ:UserName"];
        opt.Password = config["RabbitMQ:Password"];
        opt.HostName = config["RabbitMQ:HostName"];
        opt.Port =int.Parse(config["RabbitMQ:Port"]);
        opt.VirtualHost = config["RabbitMQ:VHost"];           
    });
    a.ConsumerThreadCount = 20;
    //失败重试失败后，触发动作
    a.FailedThresholdCallback = e =>  Console.WriteLine($"处理异常：faildInfo={e.ToString()}");
    a.DefaultGroupName = "booCAP.WebTest";
    a.Version = "";
    a.UseDashboard();
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
