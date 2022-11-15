using Boo.EventBus.EventBusCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.ConsoleApp.RabbitMQEvent
{
    public class RabbitMQTestService
    {
        IEventBus _eventBus;
        public RabbitMQTestService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Test()
        {
            var orderHandler = new OrderGeneratorHandler();
            _eventBus.Subscribe(typeof(OrderGeneratorEvent),new DefaultEventHandlerFactory(orderHandler));
            //_eventBus.Subscribe(typeof(OrderGeneratorEvent), new DefaultEventHandlerFactory(new OrderGeneratorHandler2()));
            //_eventBus.Subscribe(typeof(UserGeneratorEvent),new DefaultEventHandlerFactory(new UserGeneratorHandler()));
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    _eventBus.PublishAsync(new OrderGeneratorEvent { OrderId = Guid.NewGuid().ToString() });
                    //_eventBus.PublishAsync(new UserGeneratorEvent { Id = int.Parse(Console.ReadLine()) });
                }
            });
        }
    }
}
