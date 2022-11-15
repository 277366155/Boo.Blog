using System.Threading.Tasks;

namespace Boo.EventBus.EventBusCore
{
    /// <summary>
    /// 事件处理者接口
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<in TEvent> : IHandler //where TEvent: IEvent
    {
        Task InvokeAsync(TEvent eventData);
    }
}
