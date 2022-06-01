using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Boo.Blog.ToolKits.Cache
{
    public static partial class CSRedisCoreExtension
    {
        public static IServiceCollection AddCSRedisCore(this IServiceCollection services, IEnumerable<RedisHandlerOption> options)
        {
            services.AddSingleton<IRedisHandler>(new RedisHandler(options));
            //InitRedisClient(option);
            return services;
        }

        //private static CSRedisClient InitRedisClient(RedisHandlerOption option)
        //{
        //    CSRedisClient client = null;
        //    if (option.Single)
        //    {
        //        client = new CSRedisClient(option.Connect);
        //    }
        //    else
        //    {
        //        client = new CSRedisClient(option.Config, option.Hosts);
        //    }
        //    RedisHelper.Initialization(client);
        //    return client;
        //}
    }
}
