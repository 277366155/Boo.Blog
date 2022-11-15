using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPTest.ConsoleApp.Services
{
    public interface ISubscriberService
    {
        Task CheckReceivedMessageAsync(MessageModel msg);
    }

}
