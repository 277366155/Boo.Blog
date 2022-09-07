using Boo.Blog.ToolKits.Configurations;
using Grpc.Core;
using Grpc.Net.Client;

namespace ConsoleGrpcClient01
{
    public class ServerStreamClient
    {
        public static async Task InitConnectAsync()
        {
            GrpcChannel channel = null;
            try
            {
                channel = GrpcChannel.ForAddress(AppSettings.Root["ServiceRemote"]);

                var client = new User.UserClient(channel);
                Metadata header = new Metadata();
                Console.Write("输入token：");
                header.Add("token", Console.ReadLine());
                var requestMsg = new LongMessage { Value = 1 };
                var response = client.GetUserInfoById(requestMsg, header);
                //var cts = new CancellationTokenSource();
                //await Task.Factory.StartNew(async () =>
                //  {
                //      try
                //      {
                //          while (await response.ResponseStream.MoveNext())
                //          {
                //              var msg = response.ResponseStream.Current.Value;
                //              Console.WriteLine(msg);
                //          }
                //      }
                //      catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                //      {
                //          Console.WriteLine("连接断开[Cancelled]");
                //      }
                //      catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                //      {
                //          Console.WriteLine($"连接异常[Unavailable]");
                //          InitConnectAsync().Wait();
                //          //外面无法捕捉到task内部的exception
                //          throw ex;
                //      }
                //      catch (Exception ex)
                //      {
                //          Console.WriteLine($"异常：{ex.Message}\r\n{ex.StackTrace}");
                //      }
                //  });
                Console.WriteLine("信息接收中：");
                try
                {
                    while (await response.ResponseStream.MoveNext())
                    {
                        var msg = response.ResponseStream.Current.Value;
                        Console.WriteLine(msg);
                    }
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                {
                    Console.WriteLine("连接断开[Cancelled]");
                }
                catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
                {
                    Console.WriteLine($"连接异常[Unavailable]");
                    InitConnectAsync().Wait();
                    //外面无法捕捉到task内部的exception
                    throw ex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"异常：{ex.Message}\r\n{ex.StackTrace}");
                }                
                Thread.Sleep(10 * 1000);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"异常信息：{ex.Message}");
                //Thread.Sleep(3000);
                //InitConnectAsync().Wait();
            }
            finally
            {
                channel.Dispose();
            }
        }

    }
}
