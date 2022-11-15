using DotNetCore.CAP;
using System.Text.Json;

namespace CAPTest.ConsoleApp.Services
{
    public class SubscriberService : ISubscriberService, ICapSubscribe
    {
        [CapSubscribe(Constants.MQShowUrl,true,Group = "booCAP.WebTest")]
        public async Task CheckReceivedMessageAsync(MessageModel msg)
        {
           await Console.Out.WriteLineAsync(JsonSerializer.Serialize(msg));
        }
    }
}
