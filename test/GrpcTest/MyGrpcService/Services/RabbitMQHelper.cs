using Boo.Blog.ToolKits.Configurations;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace MyGrpcService
{
    public class RabbitMQHelper
    {
        static IModel channel;
        static object lockObj = new object();
        public static IModel Channel
        {
            get
            {
                if (channel == null)
                {
                    lock (lockObj)
                    {
                        if (channel == null)
                        {
                            channel = GetConnection().CreateModel();
                        }
                    }
                }
                return channel;
            }
        }

        /// <summary>
        /// 获取RabbitMQ连接对象方法（创建与RabbitMQ的连接）
        /// </summary>
        /// <returns></returns>
        private static IConnection GetConnection()
        {
            //创建连接工厂【设置相关属性】
            var connectionFactory = new ConnectionFactory()
            {
                //设置IP
                HostName = AppSettings.Root["RabbitMQConnect:HostName"],//RabbitMQ地址
                Port = Convert.ToInt32(AppSettings.Root["RabbitMQConnect:Port"]),//端口
                VirtualHost = AppSettings.Root["RabbitMQConnect:VHost"],//RabbitMQ中要请求的VirtualHost名称
                UserName = AppSettings.Root["RabbitMQConnect:UserName"],
                Password = AppSettings.Root["RabbitMQConnect:Password"],//RabbitMQ用户密码
                HandshakeContinuationTimeout = TimeSpan.FromSeconds(30),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(100)
            };
            //通过工厂创建连接对象
            return connectionFactory.CreateConnection();
        }

        public static IModel GetQueryChannel(string queueName)
        {
            //var connection = GetConnection();
            //var channel = connection.CreateModel();
            var exchange = AppSettings.Root["RabbitMQConnect:Exchange"];
            var bindRoutingKey = queueName + "_routing";
            //在MQ上定义一个队列，如果名称相同不会重复创建
            Channel.ExchangeDeclare(exchange, "direct", true, false, null);
            Channel.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>()
            {
                { "x-message-ttl",1000 * 60 * 30 }
            });
            Channel.QueueBind(queueName, exchange, bindRoutingKey);
            Channel.BasicQos(0, 1, false);

            return Channel;
        }

    }
}
