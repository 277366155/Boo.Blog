using EventBusCore.ConsoleApp;
using EventBusCore.ConsoleApp.Events;

var handlerEvents = new Type[] { typeof(UserAddedEventsHandlersSendMessage), typeof(UserAddedEventHandlerSendRedbags) };
//EventBus.InitInstance(typeof(UserGeneratorEvent), handlerEvents);
EventBus.InitInstance(typeof(OrderGeneratorEvent), handlerEvents);
EventBus.Instance.Publish(new UserGeneratorEvent { Id = Guid.NewGuid() }, (tEvent, result, ex) => { Console.WriteLine("用户订阅事件处理完成"); });
EventBus.Instance.Publish(new OrderGeneratorEvent { OrderId = Guid.NewGuid() }, (tEvent, result, ex) => { Console.WriteLine("订单订阅事件处理完成"); });
Console.WriteLine("publish发送完成");
Console.ReadLine();
