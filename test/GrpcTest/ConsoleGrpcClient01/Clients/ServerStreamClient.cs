using Boo.Blog.ToolKits.Configurations;
using Grpc.Core;
using Grpc.Net.Client;
using static ConsoleGrpcClient01.User;

namespace ConsoleGrpcClient01
{
    public class ServerStreamClient : GrpcServiceClientBase<UserClient>
    {
        UserClient client;
        static object _lock = new object();
        protected override UserClient Client
        {
            get
            {
                if (client == null)
                {
                    lock (_lock)
                    {
                        if (client == null)
                        {
                            client = new UserClient(Channel);
                        }
                    }
                }
                return client;
            }
        }

        public async Task InitConnectAsync(string token)
        {
            try
            {
                Metadata header = new Metadata();

                header.Add("token", token);
                var requestMsg = new LongMessage { Value = 1 };
                var getUserInfoStream = Client.GetUserInfoById(header);
                var task1 = await Task.Factory.StartNew(async () =>
                 {
                     while (true)
                     {
                         await Task.Delay(10 * 1000);
                         await getUserInfoStream.RequestStream.WriteAsync(requestMsg);
                     }
                 });
                Func<Task> func = null;
                func = async () => {
                    var i = 0;
                    while (i < 100)
                    {
                        try
                        {
                            while (await getUserInfoStream.ResponseStream.MoveNext())
                            {
                                var msg = getUserInfoStream.ResponseStream.Current.Value;
                                Console.WriteLine(msg);
                            }
                        }
                        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                        {
                            Console.WriteLine($"token=[{token}]，连接断开[Cancelled]");
                        }
                        catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                        {
                            Console.WriteLine($"token=[{token}]，连接异常[Unavailable]");                            
                            //InitConnectAsync(token).GetAwaiter().GetResult();
                            ////外面无法捕捉到task内部的exception
                            //throw ex;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"token=[{token}]，异常：{ex.Message}\r\n{ex.StackTrace}");
                        }
                        Thread.Sleep(10 * 1000);
                        i++;
                    }
                };

                var task2= await Task.Factory.StartNew(async () =>
                {
                    await func.Invoke();
                });

                Task.WaitAll(new Task[] { task1, task2 });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常信息：{ex.Message}");
            }
            //finally
            //{
            //    Channel.Dispose();
            //}
        }

    }
}
