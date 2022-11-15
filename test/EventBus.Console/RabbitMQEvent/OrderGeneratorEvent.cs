using Boo.EventBus.EventBusCore;

namespace EventBus.ConsoleApp.RabbitMQEvent
{
    public class OrderGeneratorEvent:IEvent
    {
        public string OrderId { get; set; }
    }
}
