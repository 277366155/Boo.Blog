using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbotSpider
{
    public class RedisQueue
    {
        static string key;
        static RedisQueue()
        {
            key = "queue.bill";
            RedisHelper.Initialization(new CSRedis.CSRedisClient("127.0.0.1:6379,defaultdatabase=0"));
        }

        public static void PushTest<T>(T msg)
        {
            RedisHelper.RPush(key,msg);
            Console.WriteLine($"推送信息：{msg}");
        }

        public static  void PopTest()
        {
            var data =RedisHelper.LPop(key);
            Console.WriteLine(data);
        }
    }
}
