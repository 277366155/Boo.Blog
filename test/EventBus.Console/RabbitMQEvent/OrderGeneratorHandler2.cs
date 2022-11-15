using Boo.Blog.ToolKits.Extensions;
using Boo.EventBus.EventBusCore;

namespace EventBus.ConsoleApp.RabbitMQEvent
{
    public class OrderGeneratorHandler2 : IEventHandler<OrderGeneratorEvent>
    {
        public Task InvokeAsync(OrderGeneratorEvent eventData)
        {
            Console.WriteLine($"OrderGeneratorHandler2也接收到信息：{eventData.ToJson()}");
            return Task.CompletedTask;
        }
    }
}
