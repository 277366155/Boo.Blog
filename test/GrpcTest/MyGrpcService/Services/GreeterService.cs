using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGrpcService.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public static Dictionary<string, Queue<string>> dicQueue = new Dictionary<string, Queue<string>>();
        static object createQueueLocker = new object();
        static object dequeueLocker = new object();

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<StringMessage> SayHello(StringMessage request, ServerCallContext context)
        {
            var clientIP = SplitKeyStrGrpcContextPeer(context.Peer);
            return Task.FromResult(new StringMessage { Value = $"Hello { request.Value}.[from: {clientIP.Item1}-{clientIP.Item2}] " });
        }

        public override async Task Bothway(IAsyncStreamReader<StringMessage> requestStream, IServerStreamWriter<StringMessage> responseStream, ServerCallContext context)
        {
            string msg = String.Empty;
            string key = String.Empty;


            while (!context.CancellationToken.IsCancellationRequested)
            {
                //ѭ��ִ��moveNext()��������տͻ��˵�������
                if (await requestStream.MoveNext())
                {
                    msg = requestStream.Current.Value;
                    key = context.RequestHeaders.Get("token").Value;

                    if (!dicQueue.ContainsKey(key))
                    {
                        lock (createQueueLocker)
                        {
                            if (!dicQueue.ContainsKey(key))
                                dicQueue.Add(key, new Queue<string>());
                        }
                    }
                    await responseStream.WriteAsync(new StringMessage { Value = $"�յ����Կͻ���[token={key}]��msg��{msg}" });
                }

                if (!context.CancellationToken.IsCancellationRequested && dicQueue[key].Count() > 0)
                {
                    lock (dequeueLocker)
                    {
                        if (!context.CancellationToken.IsCancellationRequested && dicQueue[key].Count() > 0)
                        {
                            responseStream.WriteAsync(new StringMessage()
                            {
                                Value = $"key={key}��value={dicQueue[key].Dequeue()}��address={context.Peer}"
                            }).Wait();
                        }
                    }
                }
            }
            //����whileѭ��ʱ�������ͻ����ѶϿ����ӣ���ʱ��������е���Ϣ��
            //��ֹ�´�����ʱ���ȡ�����۵Ķ�����Ϣ��
            dicQueue[key].Clear();
        }

        private Tuple<string, int> SplitKeyStrGrpcContextPeer(string strKey)
        {
            Tuple<string, int> tuple = new Tuple<string, int>("", 0);
            string[] res = strKey.Split(':');
            if (res.Length < 2)
            {
                return tuple;
            }
            int lenth = res.Length;
            int port;
            if (!int.TryParse(res[lenth - 1], out port))
            {
                return tuple;
            }

            tuple = new Tuple<string, int>(res[lenth - 2], port);

            return tuple;
        }
    }

}