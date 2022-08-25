using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGrpcClient01.Clients
{
    public class GreeterClient: Greeter.GreeterClient
    {
        public override AsyncUnaryCall<StringMessage> SayHelloAsync(StringMessage request, CallOptions options)
        {
            return base.SayHelloAsync(request, options);
        }
        public override StringMessage SayHello(StringMessage request, CallOptions options)
        {
            return base.SayHello(request, options);
        }
        public override AsyncDuplexStreamingCall<StringMessage, StringMessage> Bothway(Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.Bothway(headers, deadline, cancellationToken);
        }
    }

    public class UserClient : User.UserClient
    { 
    
    }
}
