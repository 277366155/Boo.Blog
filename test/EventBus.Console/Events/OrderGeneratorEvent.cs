namespace EventBusCore.ConsoleApp.Events
{
    public class OrderGeneratorEvent : IEvent
    {
        public Guid OrderId { get; set; }
    }
}
