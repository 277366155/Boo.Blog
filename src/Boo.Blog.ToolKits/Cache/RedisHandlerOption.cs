using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.ToolKits.Cache
{
    public enum RedisType
    { 
        Default=0,
        Common
    }
    public class RedisHandlerOption
    {
        public RedisType RedisType { get; set;}
        public bool Single { get; set; } = true;
        public string Connect { get; set; }
        public string Config { get; set; }
        public string[] Hosts { get; set; }
    }
}
