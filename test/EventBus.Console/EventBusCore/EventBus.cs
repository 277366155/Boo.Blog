using System.Diagnostics.CodeAnalysis;

namespace EventBusCore.ConsoleApp
{
    public class EventBus
    {
        /// <summary>
        /// 事件总线对象
        /// </summary>
        static EventBus _eventBus;
        /// <summary>
        /// 领域模型事件句柄集合
        /// </summary>
        static Dictionary<Type, List<object>> _dicEventHandler = new Dictionary<Type, List<object>>();
        /// <summary>
        /// 附加领域模型处理句柄时，锁对象
        /// </summary>
        readonly object _syncLock = new object();

        /// <summary>
        /// 单例事件总线
        /// </summary>
        public static EventBus Instance
        {
            get
            {
                return _eventBus ?? (_eventBus = new EventBus());
            }
        }
        /// <summary>
        /// 初始化事件总线
        /// </summary>
        /// <param name="publishEventType">发布事件类型</param>
        /// <param name="subscribedEventTypes">订阅事件集合</param>
        /// <returns></returns>
        public static EventBus InitInstance([NotNull] Type publishEventType, [NotNull] IEnumerable<Type> subscribedEventTypes)
        {
            if (publishEventType.GetInterfaces().First() != typeof(IEvent) || subscribedEventTypes.Any(a => a.GetInterfaces().First().Name != typeof(IEventHandler<IEvent>).Name))
            {
                throw new ArgumentException("参数类型不匹配");
            }
            if (_eventBus == null)
            {
                var handlers = new List<object>();

                foreach (var subscribedEvtType in subscribedEventTypes)
                {
                    handlers.Add(Activator.CreateInstance(subscribedEvtType));
                }
                _dicEventHandler[publishEventType] = handlers;

                _eventBus = new EventBus();
            }
            return _eventBus;
        }

        readonly Func<object, object, bool> eventHandlerEquals = (o1, o2) =>
        {
            return o1.GetType() == o2.GetType();
        };
        #region 订阅事件
        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            lock (_syncLock)
            {
                var eventType = typeof(TEvent);
                //如果领域模型在事件总线中已注册
                if (_dicEventHandler.ContainsKey(eventType))
                {
                    var handler = _dicEventHandler[eventType];
                    if (handler != null)
                    {
                        handler.Add(eventHandler);
                    }
                    else
                    {
                        handler = new List<object> { eventHandler };
                    }
                }
                else
                {
                    _dicEventHandler.Add(eventType, new List<object> { eventHandler });
                }
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            Subscribe(new ActionEventHandler<TEvent>(action));
        }

        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : IEvent
        {
            foreach (var eventHandler in eventHandlers)
            {
                Subscribe<TEvent>(eventHandler);
            }
        }
        #endregion 订阅事件

        #region 发布事件
        public void Publish<TEvent>(TEvent tEvent, Action<TEvent, bool, Exception> callback) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);
            if (_dicEventHandler.ContainsKey(eventType) && _dicEventHandler[eventType] != null && _dicEventHandler[eventType].Count > 0)
            {
                var handlers = _dicEventHandler[eventType];
                try
                {
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        if (eventHandler is null)
                            continue;
                        eventHandler.Handle(tEvent);
                        callback(tEvent, true, null);
                    }
                }
                catch (Exception ex)
                {
                    callback(tEvent, false, ex);
                }
            }
        }
        #endregion 发布事件

        #region 取消订阅
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : IEvent
        {
            lock (_syncLock)
            {
                var eventType = typeof(TEvent);
                if (_dicEventHandler.ContainsKey(eventType))
                {
                    var handlers = _dicEventHandler[eventType];
                    if (handlers != null && handlers.Exists(a => eventHandlerEquals(a, eventHandler)))
                    {
                        var handlerToRemove = handlers.First(a => eventHandlerEquals(a, eventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }
        #endregion 取消订阅

    }
}
