using CAPTest.ConsoleApp.Services;
using DotNetCore.CAP;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
IServiceCollection sc = new ServiceCollection();
sc.AddCap(a =>
{
    a.UseRedis(config["Redis"]);
    a.UseMySql(config["MySql"]);
    a.UseDashboard();
});
sc.AddLogging();

sc.AddSingleton<ISubscriberService, SubscriberService>();
//注入自定义的发布/订阅记录表名称
sc.AddSingleton<IStorageInitializer,MyTableInitalizer>();
var sp = sc.BuildServiceProvider();
await sp.GetService<IBootstrapper>().BootstrapAsync();
var capPublisher = sp.GetService<ICapPublisher>();
capPublisher.PublishDelay(TimeSpan.FromSeconds(10), Constants.MQShowUrl, new MessageModel { Id = 1, Content = "firest test message." });
capPublisher.Publish(Constants.MQShowUrl, new MessageModel { Id = 2, Content = "second test message." });

Console.ReadLine();