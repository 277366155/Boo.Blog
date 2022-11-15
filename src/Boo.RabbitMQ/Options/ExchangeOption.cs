using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.RabbitMQ.Options
{
    public class ExchangeOption
    {
        public string Name { get; set;}
        public string Type { get; set; } = ExchangeType.Direct;
        /// <summary>
        /// 持久化
        /// </summary>
        public bool Durable { get; set; } = true;

        public bool AutoDelete { get; set; }

        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();

    }
}
