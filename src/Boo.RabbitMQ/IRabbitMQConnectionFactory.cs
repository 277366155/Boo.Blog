using RabbitMQ.Client;

namespace Boo.RabbitMQ
{
    public interface IRabbitMQConnectionFactory
    {
        IConnection GetConnection();
    }
}
