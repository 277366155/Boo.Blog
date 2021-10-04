using CSRedis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Boo.Blog.ToolKits.Cache
{
    public static class RedisHelperExtension
    {
        public static IServiceCollection AddCSRedisCore(this IServiceCollection services, RedisHandlerOption option)
        {
            CSRedisClient client = null;
            if (option.Single)
            {
                client = new CSRedisClient(option.Connect);
            }
            else
            {
                client = new CSRedisClient(option.Config, option.Hosts);
            }
            RedisHelper.Initialization(client);

            services.AddSingleton(RedisHelper.Instance);
            return services;
        }

    }
}
