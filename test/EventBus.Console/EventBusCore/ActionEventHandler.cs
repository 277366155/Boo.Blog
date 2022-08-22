namespace EventBusCore.ConsoleApp
{
    public class ActionEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        readonly Action<TEvent> _action;
        public ActionEventHandler(Action<TEvent> action)
        {
            _action = action;
        }
        public void Handle(TEvent message)
        {
            _action(message);
        }
    }
}
