using Boo.RabbitMQ.Options;
using RabbitMQ.Client;

namespace Boo.RabbitMQ
{
    public class RabbitMQConnectionFactory: IRabbitMQConnectionFactory
    {
        public RabbitMQOption RabbitMQOption { get; }

        private IConnection _instance;

        private static object lockObj = new object();
        public RabbitMQConnectionFactory(RabbitMQOption option)
        {
            RabbitMQOption = option;
        }

        public IConnection GetConnection()
        {
            if (_instance != null)
                return _instance;
            lock (lockObj)
            {
                if (_instance == null)
                {
                    _instance = new ConnectionFactory()
                    {
                        HostName = RabbitMQOption.HostName,
                        Port = RabbitMQOption.Port,
                        UserName = RabbitMQOption.UserName,
                        Password = RabbitMQOption.Password,
                        VirtualHost = RabbitMQOption.VirtualHost,
                        AutomaticRecoveryEnabled = true,
                        DispatchConsumersAsync = true
                    }.CreateConnection();
                }
            }
            return _instance;
        }
    }
}
