using System;
using System.Threading.Tasks;

namespace Boo.EventBus.EventBusCore
{
    /// <summary>
    /// 事件总线基础接口
    /// </summary>
    public interface IEventBus
    {
        void Subscribe(Type eventType,IEventHandlerFactory handlerFactory) ;
        void Unsubscribe(Type eventType,IEventHandlerFactory handlerFactory) ;
        void UnsubscribeAll(Type eventType) ;
        Task PublishAsync<TEvent>(TEvent eventData) where TEvent : IEvent;
    }
}
