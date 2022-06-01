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
            return services;
        }
    }
}
