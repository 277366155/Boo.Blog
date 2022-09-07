using Grpc.Core;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MyGrpcService.Services
{
    public class UserService : User.UserBase
    {
        public override Task<StringMessage> GetUserNameById(LongMessage request, ServerCallContext context)
        {
            return Task.FromResult(new StringMessage { Value = $"userid={request.Value} 的用户名未找到。" });
        }

        public override async Task GetUserInfoById(LongMessage request, IServerStreamWriter<StringMessage> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"接收到消息数据：{request.Value} ");
            var token = context.RequestHeaders.Get("token").Value;
            
            await responseStream.WriteAsync(new StringMessage { Value = $"{Dns.GetHostName()}已接收到来自{context.Peer}的连接。" });
            while (!context.CancellationToken.IsCancellationRequested)
            {
                if (!context.CancellationToken.IsCancellationRequested &&await RedisHelper.ExistsAsync(token))
                {
                    var queueMsg=await  RedisHelper.LPopAsync(token);

                 await   responseStream.WriteAsync(new StringMessage()
                    {
                        Value = $"key={token}，value={queueMsg}，address={context.Peer}，serviceHostName={Dns.GetHostName()}"
                    });
                }
            }

        }
    }
}
