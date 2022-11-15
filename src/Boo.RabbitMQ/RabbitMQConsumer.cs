using Boo.RabbitMQ.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Boo.RabbitMQ
{
    public class RabbitMQConsumer : IRabbitMQConsumer
    {
        private readonly IRabbitMQConnectionFactory _connectionFactory;
        private bool _autoAck;
        protected ExchangeOption ExchangeOption { get; set; }
        protected QueueOption QueueOption { get; set; }
        protected ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>> Callbacks { get; set; }
        public IModel Channel { get; set; }

        public RabbitMQConsumer(ExchangeOption exchangeOption, QueueOption queueOption, IRabbitMQConnectionFactory connectionFactory)
        {
            ExchangeOption = exchangeOption;
            QueueOption = queueOption;
            _connectionFactory = connectionFactory;
            Callbacks = new ConcurrentBag<Func<IModel, BasicDeliverEventArgs, Task>>();
        }

        /// <summary>
        /// 初始化消费端。
        /// 声明交换机、队列、额外绑定路由
        /// </summary>
        /// <param name="autoAck"></param>
        /// <param name="routingKeys"></param>
        /// <returns></returns>
        public IRabbitMQConsumer Initialize(bool autoAck=false,params string[] routingKeys)
        {
            _autoAck = autoAck;
            Channel?.Dispose();
            Channel = _connectionFactory.GetConnection().CreateModel();
            Channel.ExchangeDeclare(ExchangeOption.Name, ExchangeOption.Type, ExchangeOption.Durable, ExchangeOption.AutoDelete, ExchangeOption.Arguments);
            Channel.QueueDeclare(QueueOption.Name, QueueOption.Durable, QueueOption.Exclusive, QueueOption.AutoDelete, QueueOption.Arguments);
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += MessageReceivedHandlerAsync;
            //声明消费者队列名，设置ack
            Channel.BasicConsume(QueueOption.Name, autoAck, consumer: consumer);

            if (routingKeys != null)
            {
                foreach (var routingKey in routingKeys)
                {
                    Bind(routingKey);
                }
            }
            return this;
        }

        /// <summary>
        /// 消息消费时回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="basicDeliverEventArgs"></param>
        /// <returns></returns>
        public virtual async Task MessageReceivedHandlerAsync(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            try
            {
                foreach (var callback in Callbacks)
                {
                    await callback(Channel, basicDeliverEventArgs);
                }
                if(!_autoAck)
                    Channel.BasicAck(basicDeliverEventArgs.DeliveryTag, multiple: false);
            }
            catch(Exception e)
            {
                try
                {
                    if (!_autoAck)
                        Channel.BasicNack(basicDeliverEventArgs.DeliveryTag, multiple: false, requeue: true);
                }
                catch (Exception ex)
                {
                    //todo：log记录
                    Console.WriteLine(ex);
                }
                //todo：log记录
                Console.WriteLine($"意外错误:{e}");
            }
        }

        /// <summary>
        /// 注册消费端处理逻辑
        /// </summary>
        /// <param name="callback"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnMessageReceived(Func<IModel, BasicDeliverEventArgs, Task> callback)
        {
            Callbacks.Add(callback);
        }

        /// <summary>
        /// 绑定队列和路由key（以Type的FullName作为RoutingKey）
        /// </summary>
        /// <param name="eventType"></param>
        public void Bind(Type eventType)
        {
            Bind(eventType.FullName);
        }
        /// <summary>
        /// 解绑队列和路由key（以Type的FullName作为RoutingKey）
        /// </summary>
        /// <param name="eventType"></param>
        public void Unbind(Type eventType)
        {
            Unbind(eventType.FullName);
        }

        public void Bind(string  routingKey)
        {
            Channel.QueueBind(QueueOption.Name, ExchangeOption.Name, routingKey);
        }
        public void Unbind(string routingKey)
        {
            Channel.QueueUnbind(QueueOption.Name, ExchangeOption.Name, routingKey);
        }
    }
}
