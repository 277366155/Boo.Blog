using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.RabbitMQ.Options
{
    public class RabbitMQOption
    {
        public string HostName { get; set; } 
        public int Port { get; set; }
        public string UserName { get; set;}
        public string Password { get; set; }
        public string VirtualHost { get; set; } = "/";
    }
}
