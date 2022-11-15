using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.CAP
{
    public class CAPOption
    {
        public RabbitMQOpt RabbitMQOpt { get; set; }
        public string MySqlConnection { get; set; }
    }

    public class RabbitMQOpt
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string DefaultGroupName { get; set; }
    }
}
