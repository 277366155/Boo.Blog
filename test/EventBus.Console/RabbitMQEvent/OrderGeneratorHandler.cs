using Boo.Blog.ToolKits.Extensions;
using Boo.EventBus.EventBusCore;

namespace EventBus.ConsoleApp.RabbitMQEvent
{
    public class OrderGeneratorHandler : IEventHandler<OrderGeneratorEvent>
    {
        public async Task InvokeAsync(OrderGeneratorEvent eventData)
        {
           await Task.Delay(300);
            Console.WriteLine($"{this.GetHashCode()}-OrderGeneratorHandler接收到信息：{eventData.ToJson()}");
        }
    }
}
