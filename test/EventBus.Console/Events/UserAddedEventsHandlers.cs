namespace EventBusCore.ConsoleApp.Events
{
    public class UserAddedEventsHandlersSendEmail : IEventHandler<UserGeneratorEvent>
    {
        public void Handle(UserGeneratorEvent evt)
        {
            Console.WriteLine($"用户{evt.Id}已发送邮件通知。");
        }
    }

    public class UserAddedEventsHandlersSendMessage : IEventHandler<UserGeneratorEvent>
    {
        public void Handle(UserGeneratorEvent evt)
        {
            if (evt is null)
                return;
            Console.WriteLine($"用户{evt.Id}已发送短信通知。");
        }
    }

    public class UserAddedEventHandlerSendRedbags : IEventHandler<UserGeneratorEvent>, IEventHandler<OrderGeneratorEvent>
    {
        public void Handle(UserGeneratorEvent evt)
        {
            if (evt is null)
                return;
            Console.WriteLine($"用户{evt.Id}注册红包已发放。");
        }

        public void Handle(OrderGeneratorEvent evt)
        {
            if (evt is null)
                return;
            Console.WriteLine($"订单{evt.OrderId}已提交。");
        }
    }
}
