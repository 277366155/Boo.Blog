using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.EventBus.EventBusCore
{
    public class DefaultEventHandlerFactory : IEventHandlerFactory
    {
        IHandler _handlerInstance;

        public DefaultEventHandlerFactory(IHandler handlerInstance)
        {
            _handlerInstance = handlerInstance;
        }

        public IHandler GetHandler()
        {
            return _handlerInstance;
        }
    }
}
