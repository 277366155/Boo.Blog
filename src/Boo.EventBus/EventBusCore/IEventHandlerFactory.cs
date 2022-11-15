namespace Boo.EventBus.EventBusCore
{
    /// <summary>
    /// 事件处理者工厂接口
    /// </summary>
    public interface IEventHandlerFactory
    {
        IHandler GetHandler();
    }
}
