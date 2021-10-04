using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.ToolKits.Cache
{
    public class RedisHandlerOption
    {
        public bool Single { get; set; } = true;
        public string Connect { get; set; }
        public string Config { get; set; }
        public string[] Hosts { get; set; }
    }
}
