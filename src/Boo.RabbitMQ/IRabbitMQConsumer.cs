using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Boo.RabbitMQ
{
    public interface IRabbitMQConsumer
    {
        IRabbitMQConsumer Initialize(bool autoAck=false,params string[] routingKeys);

        void Bind(Type eventType);

        void Unbind(Type eventType);

        void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback);
    }
}
