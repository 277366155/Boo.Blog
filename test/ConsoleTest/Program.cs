
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.Extensions.Configuration;


var task = Task.Factory.StartNew(() => Console.WriteLine(123));

Console.Read();

RedisHelper.Initialization(new CSRedis.CSRedisClient(AppSettings.Root["redisConnect"]));
Console.Write("请输入key：");
var key=Console.ReadLine();
Console.Write("请输入token：");
var token=Console.ReadLine();
await RedisHelper.EvalAsync("if(redis.call('hexists',KEYS[1],ARGV[1])==1)  then local val=tonumber(redis.call('HGET',KEYS[1],ARGV[1])) redis.call('HSET',KEYS[1],ARGV[1],val+1)  else redis.call('HSET',KEYS[1],ARGV[1],1)  end ", key, token);
Console.Write("插入数据完成，任意键继续");
Console.ReadKey();
await RedisHelper.EvalAsync($@"if(redis.call('hexists',KEYS[1],ARGV[1])==1)  then local val=tonumber(redis.call('HGET',KEYS[1],ARGV[1])) if(val<=1) then redis.call('HDEL',KEYS[1],ARGV[1]) redis.call('del',ARGV[1]) else redis.call('HSET',KEYS[1],ARGV[1],val-1)  end  end ", key, token);
Console.Write("删除数据完成，任意键继续");

//try
//{
//    var opts = AppSettings.Root.GetSection("Redis").Get<IEnumerable<RedisHandlerOption>>();
//    var redisHandler = new RedisHandler(opts);
//    for (var i = 0; i < 10; i++)
//    {
//        await redisHandler.SetAsync("testkey" + i, "testValue" + i, 200);
//        Console.WriteLine(await redisHandler.GetAsync("testkey" + i));
//        Thread.Sleep(1000);
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
Console.Read();

//Number n = new Number(2, 2.0);
//Number n2 = new Number(3, 3.0);
// var num= n * n2;
//Console.WriteLine(num);
//Console.ReadLine();


// Define other methods and classes here
//public struct Number
//{
//    public int intV;
//    public double doubleV;
//    public Number(int iVal, double dVal)
//    {
//        intV = iVal;
//        doubleV = dVal;
//    }

//    public Number(int val) : this(val, 0)
//    { }

//    public Number(double val) : this(0, val)
//    { }

//    public static Number operator *(Number num, Number num2)
//    {
//        var t = num.intV*num2.intV;
//        var t2 = num.doubleV* num2.doubleV;
//        var newt = new Number(t, t2);
//        return newt;
//    }

//    public override string ToString()
//    {
//        return $"intV={intV},doubleV={doubleV}";

//    }
//}