using Boo.Blog.ToolKits.Extensions;
using Boo.EventBus.EventBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.ConsoleApp.RabbitMQEvent
{
    public class UserGeneratorHandler : IEventHandler<UserGeneratorEvent>
    {
        public Task InvokeAsync(UserGeneratorEvent eventData)
        {
            Console.WriteLine($"UserGeneratorHandler接收到信息：{eventData.ToJson()}");
            return Task.CompletedTask;
        }
    }
}
