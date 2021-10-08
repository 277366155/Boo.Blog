using CSRedis;
using Microsoft.Extensions.DependencyInjection;

namespace Boo.Blog.ToolKits.Cache
{
    public static partial class CSRedisCoreExtension
    {
        public static IServiceCollection AddCSRedisCore(this IServiceCollection services, RedisHandlerOption option)
        {
            InitRedisClient(option);
            return services;
        }

        private static void InitRedisClient(RedisHandlerOption option)
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
        }
    }
}
