using Boo.Blog.ToolKits.Configurations;
using Grpc.Core;
using Grpc.Net.Client;

namespace ConsoleGrpcClient01
{
    public abstract class GrpcServiceClientBase<T> where T : ClientBase<T>
    {
        protected static GrpcChannel Channel = GrpcChannel.ForAddress(AppSettings.Root["ServiceRemote"]);
        protected abstract T Client { get; }
    }
}
