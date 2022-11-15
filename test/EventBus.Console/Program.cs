//using EventBusCore.ConsoleApp;
//using EventBusCore.ConsoleApp.Events;

//var handlerEvents = new Type[] { typeof(UserAddedEventsHandlersSendMessage), typeof(UserAddedEventHandlerSendRedbags) };
////EventBus.InitInstance(typeof(UserGeneratorEvent), handlerEvents);
//EventBus.InitInstance(typeof(OrderGeneratorEvent), handlerEvents);
//EventBus.Instance.Publish(new UserGeneratorEvent { Id = Guid.NewGuid() }, (tEvent, result, ex) => { Console.WriteLine($"用户订阅事件处理完成，Id={tEvent.Id}"); });
//EventBus.Instance.Publish(new OrderGeneratorEvent { OrderId = Guid.NewGuid() }, (tEvent, result, ex) => { Console.WriteLine($"订单订阅事件处理完成，orderId={tEvent.OrderId}"); });
//Console.WriteLine("publish发送完成");
//Console.ReadLine();

using Boo.EventBus;
using Boo.EventBus.EventBusCore;
using Boo.RabbitMQ;
using Boo.RabbitMQ.Options;
using EventBus.ConsoleApp.RabbitMQEvent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var conf = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", true, true)
    .Build();
var sc = new ServiceCollection();
sc.AddSingleton(conf.GetSection("RabbitMQ:RabbitMQOption").Get<RabbitMQOption>());
sc.AddSingleton(conf.GetSection("RabbitMQ:ExchangeOption").Get<ExchangeOption>());
sc.AddSingleton(conf.GetSection("RabbitMQ:QueueOption").Get<QueueOption>());
sc.AddScoped<IRabbitMQConsumer, RabbitMQConsumer>();
sc.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
sc.AddScoped<IEventBus, RabbitMQEventBus>();
sc.AddScoped<RabbitMQTestService>();
var sp = sc.BuildServiceProvider();
for (var i = 0; i < 20; i++)
{
    using (var scope = sp.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<RabbitMQTestService>();
        service.Test();
    }
}
Console.ReadLine();