using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.RabbitMQ.Options
{
    public class QueueOption
    {
        public string Name { get; set; }

        public bool Durable { get; set; } = true;

        public bool Exclusive { get; set; }

        public bool AutoDelete { get; set; }

        public IDictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}
