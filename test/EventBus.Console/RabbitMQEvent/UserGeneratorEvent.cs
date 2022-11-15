using Boo.EventBus.EventBusCore;

namespace EventBus.ConsoleApp.RabbitMQEvent
{
    public class UserGeneratorEvent : IEvent
    {
        public int Id { get; set; }
    }
}
