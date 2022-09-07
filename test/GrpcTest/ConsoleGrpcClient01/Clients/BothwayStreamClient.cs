using Boo.Blog.ToolKits.Configurations;
using Grpc.Core;
using Grpc.Net.Client;

namespace ConsoleGrpcClient01
{
    public class BothwayStreamClient
    {
        public static async Task InitConnectAsync()
        {
            try
            {
                using (var channel = GrpcChannel.ForAddress(AppSettings.Root["ServiceRemote"]))
                {
                    var client = new Greeter.GreeterClient(channel);
                    Metadata header = new Metadata();
                    Console.Write("输入token：");
                    header.Add("token", Console.ReadLine());
                    var bothway = client.Bothway(header);
                    var cts = new CancellationTokenSource();
                    await bothway.RequestStream.WriteAsync(new StringMessage { Value = "0" });
                    await bothway.RequestStream.CompleteAsync();
                    Console.WriteLine("请求发送完毕");
                    Console.WriteLine("等待响应...");

                    var backTask = Task.Factory.StartNew(async () =>
                    {
                        try
                        {
                            while (await bothway.ResponseStream.MoveNext(cts.Token))
                            {
                                var back = bothway.ResponseStream.Current;
                                Console.WriteLine($"{back.Value}");
                            }
                        }
                        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
                        {
                            Console.WriteLine("Stream cancelled. ");
                        }
                    });
                    //等待响应完成
                    await backTask;

                    Console.WriteLine("模块已全部加载完毕。消息接受中。。");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常信息：" + ex.Message);
            }
        }
    }
}
