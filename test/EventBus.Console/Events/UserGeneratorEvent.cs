namespace EventBusCore.ConsoleApp.Events
{
    public class UserGeneratorEvent : IEvent
    {       
        public Guid Id { get; set; }
    }
}
