namespace EventBusCore.ConsoleApp
{
    public class ActionEventHanlder<TEvent> : IEventHandler<TEvent> where TEvent : IEvent
    {
        readonly Action<TEvent> _action;
        public ActionEventHanlder(Action<TEvent> action)
        {
            _action = action;
        }
        public void Handle(TEvent message)
        {
            _action(message);
        }
    }
}
