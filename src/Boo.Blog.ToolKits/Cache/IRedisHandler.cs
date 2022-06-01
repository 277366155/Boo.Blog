using System;
using System.Threading.Tasks;

namespace Boo.Blog.ToolKits.Cache
{
    public interface IRedisHandler
    {
        Task<string> GetAsync(string key, RedisType redisType = RedisType.Default);
        Task<T> GetAsync<T>(string key, RedisType redisType = RedisType.Default);
        Task<bool> SetAsync(string key, object value, TimeSpan timeSpan, RedisType redisType = RedisType.Default);


        Task<bool> SetAsync(string key, object value, int expireSecond = -1, RedisType redisType = RedisType.Default);

    }
}
