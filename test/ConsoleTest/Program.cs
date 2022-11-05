
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using ConsoleTest;
using Microsoft.Extensions.Configuration;

while (true)
{
    Console.WriteLine("请选择：\r\n\t1-加密\r\n\t2-解密");
    Console.WriteLine("----------------------------------------");
    var whileFlag = "1";
    var read = Console.ReadLine();

    if (read == "1")
    {
        while (whileFlag.ToLower() != "q")
        {
            Console.Write("待加密串（输入q返回菜单）：");
            whileFlag = Console.ReadLine();
            if (whileFlag.ToLower() == "q")
                continue;
            Console.Write("加密结果：");
            Console.WriteLine(SM4Util.Sm4EncryptECB(whileFlag));
            Console.Write("解密结果：");
            Console.WriteLine(SM4Util.Sm4DecryptECB(SM4Util.Sm4EncryptECB(whileFlag)));
            Console.WriteLine("----------------------------------------");
        }
    }
    else if (read == "2")
    {
        while (whileFlag.ToLower() != "q")
        {
            Console.Write("待解密串（输入q返回菜单）：");
            whileFlag = Console.ReadLine();
            if (whileFlag.ToLower() == "q")
                continue;
            Console.Write("解密结果：");
            Console.WriteLine(SM4Util.Sm4DecryptECB(whileFlag));
            Console.Write("加密结果：");
            Console.WriteLine(SM4Util.Sm4EncryptECB(SM4Util.Sm4DecryptECB(whileFlag)));
            Console.WriteLine("----------------------------------------");
        }
    }
}

Console.Read();

#region lua脚本操作redis
//RedisHelper.Initialization(new CSRedis.CSRedisClient(AppSettings.Root["redisConnect"]));
//Console.Write("请输入key：");
//var key=Console.ReadLine();
//Console.Write("请输入token：");
//var token=Console.ReadLine();
//await RedisHelper.EvalAsync("if(redis.call('hexists',KEYS[1],ARGV[1])==1)  then local val=tonumber(redis.call('HGET',KEYS[1],ARGV[1])) redis.call('HSET',KEYS[1],ARGV[1],val+1)  else redis.call('HSET',KEYS[1],ARGV[1],1)  end ", key, token);
//Console.Write("插入数据完成，任意键继续");
//Console.ReadKey();
//await RedisHelper.EvalAsync($@"if(redis.call('hexists',KEYS[1],ARGV[1])==1)  then local val=tonumber(redis.call('HGET',KEYS[1],ARGV[1])) if(val<=1) then redis.call('HDEL',KEYS[1],ARGV[1]) redis.call('del',ARGV[1]) else redis.call('HSET',KEYS[1],ARGV[1],val-1)  end  end ", key, token);
//Console.Write("删除数据完成，任意键继续");
#endregion lua脚本操作redis

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