using Grpc.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyGrpcService.Services
{
    public class UserService : User.UserBase
    {
        static Dictionary<string, EventingBasicConsumer> consumerDic = new Dictionary<string, EventingBasicConsumer>();
        static object dicLock = new object();
        static int clientCount = 0;
        //public override Task<StringMessage> GetUserNameById(LongMessage request, ServerCallContext context)
        //{
        //    return Task.FromResult(new StringMessage { Value = $"userid={request.Value} 的用户名未找到。" });
        //}
        //static Dictionary<string, int> clientCountDic = new Dictionary<string, int>();
        static object consumerDicLock = new object();
        static string guid = Guid.NewGuid().ToString();
        public override async Task GetUserNameById(LongMessage request, IServerStreamWriter<StringMessage> responseStream, ServerCallContext context)
        {
            try
            {
                lock (consumerDicLock)
                {
                    clientCount++;
                }
                var token = context.RequestHeaders.Get("token").Value;
                string msg = $"当前grpc客户端连接总数：【{clientCount}】。 {Dns.GetHostName()}已接收到来自{context.Peer}的连接。";
                Console.WriteLine(msg);
                await responseStream.WriteAsync(new StringMessage { Value = msg });
                var sw = new Stopwatch();
                sw.Start();
                //using (var channel = RabbitMQHelper.GetQueryChannel(token))
                //{
                if (!consumerDic.ContainsKey(token))
                {
                    lock (dicLock)
                    {
                        if (!consumerDic.ContainsKey(token))
                        {
                            var channel = RabbitMQHelper.GetQueryChannel(token);
                            var consumer = new EventingBasicConsumer(RabbitMQHelper.GetQueryChannel(token));
                            consumer.Received += async (s, e) =>
                            {
                                try
                                {
                                    string data = Encoding.UTF8.GetString(e.Body.ToArray());
                                    if (string.IsNullOrWhiteSpace(data))
                                    {
                                        return;
                                    }
                                    if (!context.CancellationToken.IsCancellationRequested)
                                    {
                                        await responseStream.WriteAsync(new StringMessage()
                                        {
                                            Value = $"key={token}，value={data}，address={context.Peer}，serviceHostName={Dns.GetHostName()}"
                                        });
                                        channel.BasicAck(e.DeliveryTag, false);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    channel.BasicNack(e.DeliveryTag, false, true);
                                    //todo：提示或记录异常信息
                                }
                            };
                            //consumer.ConsumerCancelled += (obj, e) => { channel.Dispose(); };
                            channel.BasicConsume(token, false, consumer);
                            sw.Stop();
                            Console.WriteLine($"token=[{token}]，rabbitmq消费端创建耗时:[{sw.ElapsedMilliseconds}ms]");
                        }
                    }
                }

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(10);
                }
            }
            finally
            {
                lock (consumerDicLock)
                {
                    clientCount--;
                }
            }
        }

        public override async Task GetUserInfoById(IAsyncStreamReader<LongMessage> requestStream, IServerStreamWriter<StringMessage> responseStream, ServerCallContext context)
        {
            var token = context.RequestHeaders.Get("token").Value;
            try
            {
                //if (!clientCountDic.Keys.Contains(token))
                //{
                //    lock (countLock)
                //    {
                //        if (!clientCountDic.Keys.Contains(token))
                //        {
                //            clientCountDic.Add(token, 0);
                //        }
                //    }
                //}
                //lock (countLock)
                //{
                //    clientCountDic[token] += 1;
                //}

                string msg = $"当前{Dns.GetHostName()}已接收到来自{context.Peer}的连接。";
                Console.WriteLine(msg);

                await responseStream.WriteAsync(new StringMessage { Value = msg });
                var sw = new Stopwatch();
                sw.Start();
                //using (var channel = RabbitMQHelper.GetQueryChannel(token))
                //{
                if (!consumerDic.ContainsKey(token))
                {
                    lock (dicLock)
                    {
                        if (!consumerDic.ContainsKey(token))
                        {
                            var channel = RabbitMQHelper.GetQueryChannel(token);
                            var consumer = new EventingBasicConsumer(RabbitMQHelper.GetQueryChannel(token));
                            consumer.Received += async (s, e) =>
                            {
                                try
                                {
                                    string data = Encoding.UTF8.GetString(e.Body.ToArray());
                                    if (string.IsNullOrWhiteSpace(data))
                                    {
                                        return;
                                    }
                                    if (!context.CancellationToken.IsCancellationRequested)
                                    {
                                        await responseStream.WriteAsync(new StringMessage()
                                        {
                                            Value = $"key={token}，value={data}，address={context.Peer}，serviceHostName={Dns.GetHostName()}"
                                        });
                                        channel.BasicAck(e.DeliveryTag, false);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    channel.BasicNack(e.DeliveryTag, false, true);
                                    //todo：提示或记录异常信息
                                }
                            };
                            //consumer.ConsumerCancelled += (obj, e) =>
                            //{
                            //    channel.BasicCancel(e.ConsumerTags.FirstOrDefault());
                            //};
                            channel.BasicConsume(token, false, consumer);
                            sw.Stop();
                            Console.WriteLine($"token=[{token}]，rabbitmq消费端创建耗时:[{sw.ElapsedMilliseconds}ms]");
                            consumerDic.Add(token, consumer);
                        }
                    }
                }
                #region 客户端流数据接收
                try
                {
                    while (!context.CancellationToken.IsCancellationRequested && await requestStream.MoveNext())
                    {
                        Console.WriteLine($"接收到token={token}，message={requestStream.Current.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine($"断开连接：token={token}");
                #endregion
            }
            finally
            {
                if (consumerDic.ContainsKey(token))
                {
                    lock(consumerDicLock)
                    {
                        if (consumerDic.ContainsKey(token))
                        {
                            RabbitMQHelper.Channel.BasicCancel(consumerDic[token].ConsumerTags[0]);
                            consumerDic.Remove(token);
                        }
                    }
                }
                //lock (countLock)
                //{
                //    clientCountDic[token] -= 1;
                //    if (clientCountDic[token] == 0)
                //        clientCountDic.Remove(token);
                //}
            }
        }

        //public async Task GetUserInfoByIdBak(LongMessage request, IServerStreamWriter<StringMessage> responseStream, ServerCallContext context)
        //{            
        //    var token = context.RequestHeaders.Get("token").Value;
        //    string msg =  $"当前grpc客户端连接总数：【{clientCount}】。 {Dns.GetHostName()}已接收到来自{context.Peer}的连接。";
        //    Console.WriteLine(msg);
        //    await responseStream.WriteAsync(new StringMessage { Value = msg });
        //    while (!context.CancellationToken.IsCancellationRequested)
        //    {
        //        if (!context.CancellationToken.IsCancellationRequested && await RedisHelper.ExistsAsync(token))
        //        {
        //            var queueMsg = await RedisHelper.LPopAsync(token);

        //            await responseStream.WriteAsync(new StringMessage()
        //            {
        //                Value = $"key={token}，value={queueMsg}，address={context.Peer}，serviceHostName={Dns.GetHostName()}"
        //            });
        //        }
        //    }         
        //}
    }
}
