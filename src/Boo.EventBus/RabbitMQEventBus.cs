using Boo.EventBus.EventBusCore;
using Boo.RabbitMQ;
using Boo.RabbitMQ.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.Json;

namespace Boo.EventBus
{
    /// <summary>
    /// rabbitMQ事件总线
    /// </summary>
    public class RabbitMQEventBus : IEventBus
    {
        readonly IRabbitMQConnectionFactory _rabbitMQConnectionFactory;
        readonly IRabbitMQConsumer _consumer;
        readonly ExchangeOption _exchangeOption;
        /// <summary>
        /// 存储不同类型的回调事件类
        /// </summary>
        readonly ConcurrentDictionary<Type, List<IEventHandlerFactory>> _handlers;
        /// <summary>
        /// 存储消息队列的routingkey 以及消息体对应的类
        /// </summary>
        readonly Dictionary<string, Type> _eventTypes;

        public RabbitMQEventBus(IRabbitMQConnectionFactory rabbitMQConnectionFactory, IRabbitMQConsumer consumer, ExchangeOption exchangeOption)
        {
            _rabbitMQConnectionFactory = rabbitMQConnectionFactory;
            _consumer = consumer;
            _exchangeOption = exchangeOption;
            _eventTypes = new Dictionary<string, Type>();
            _handlers = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            InitializeConsumer();
        }

        /// <summary>
        /// 初始化消费端
        /// </summary>
        /// <param name="callbackList"></param>
        public void InitializeConsumer(bool autoAck = false)
        {
            _consumer?.Initialize(autoAck);

            _consumer?.OnMessageReceived(ProcessEventAsync);
        }

        /// <summary>
        /// 消息推送
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(TEvent eventData) where TEvent : IEvent
        {
            var eventTypeName = typeof(TEvent).FullName;
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventData));
            using (var channel = _rabbitMQConnectionFactory.GetConnection().CreateModel())
            {
                channel.ExchangeDeclare(_exchangeOption.Name,_exchangeOption.Type,_exchangeOption.Durable,_exchangeOption.AutoDelete,_exchangeOption.Arguments);
                var props= channel.CreateBasicProperties();
                props.Persistent = true;
                channel.BasicPublish(_exchangeOption.Name,eventTypeName,props,body);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 订阅事件类型以及事件对应的处理方法
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerFactory"></param>
        public void Subscribe(Type eventType, IEventHandlerFactory handlerFactory)
        {
            if (eventType == null || handlerFactory == null)
                return;
            _handlers.GetOrAdd(eventType, _ => new List<IEventHandlerFactory>()).Add(handlerFactory);
            _consumer.Bind(eventType);
            if (!_eventTypes.ContainsKey(eventType.FullName))
                _eventTypes.Add(eventType.FullName,eventType);       
        }

        /// <summary>
        /// 取消订阅事件类型以及事件对应的处理方法
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerFactory"></param>
        public void Unsubscribe(Type eventType, IEventHandlerFactory handlerFactory)
        {
            _handlers.GetOrAdd(eventType, _ => new List<IEventHandlerFactory>()).Remove(handlerFactory);
        }
        /// <summary>
        /// 清理所有订阅
        /// </summary>
        /// <param name="eventType"></param>
        public void UnsubscribeAll(Type eventType)
        {
            _handlers.GetOrAdd(eventType, _ => new List<IEventHandlerFactory>()).Clear();
        }

        /// <summary>
        /// 执行消费端订阅逻辑
        /// 根据RoutingKey来区分不同的消息体对应的类型，进而执行对应的回调方法。
        /// 注意：这里接收到RoutingKey不匹配的类型，会被return掉之后ack，从而导致数据丢失。
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private async Task ProcessEventAsync(IModel channel, BasicDeliverEventArgs args)
        {
            var eventName = args.RoutingKey;
            _eventTypes.TryGetValue(eventName, out var eventType);
            var msg = Encoding.UTF8.GetString(args.Body.ToArray());
            if (eventType == null)
                //todo: 无匹配类型，可将此信息以及exchange\vhost\routingkey等信息，存入DB用于数据修复处理
                return;
            var eventData = JsonSerializer.Deserialize(msg, eventType);
            var handlerFactoryList = _handlers.GetOrAdd(eventType,_=>new List<IEventHandlerFactory>());
            await TriggerEventAsync(handlerFactoryList,eventType,eventData);
        }
        private async Task TriggerEventAsync(List<IEventHandlerFactory> handlerFactories, Type eventType, object eventData)
        {
            foreach (var handlerFactory in handlerFactories)
            {
                var handler = handlerFactory.GetHandler();
                if (IsAssignableToGenericType(handler.GetType().GetTypeInfo()))
                {
                    var method = typeof(IEventHandler<>)?.MakeGenericType(eventType)
                        ?.GetMethod(nameof(IEventHandler<IEvent>.InvokeAsync));
                    await (Task)(method?.Invoke(handler, new[] { eventData }) ?? Task.CompletedTask);
                }
            }
        }
        /// <summary>
        /// 是否可赋值给泛型参数
        /// </summary>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        private bool IsAssignableToGenericType(TypeInfo handlerType)
        {
            if (handlerType.IsGenericType && handlerType.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                return true;
            //遍历参数类型所实现的所有接口
            foreach (var interfaceType in handlerType.GetInterfaces())
            {
                //当前接口是否是泛型或是IEventHanlder<>
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                {
                    return true;
                }
            }

            if (handlerType.BaseType == null)
                return false;
            //如果没有结果，就再判断当前类型的的父级（递归）
            return IsAssignableToGenericType(handlerType.BaseType.GetTypeInfo());
        }
    }
}
