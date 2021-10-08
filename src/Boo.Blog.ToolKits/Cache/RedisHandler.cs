using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace Boo.Blog.ToolKits.Cache
{
    public class RedisHandler
    {
        /// <summary>
        /// 尝试从缓存查询数据，若缓存没有，就执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="expires">过期时间，单位秒</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<T> TryToGetFromCacheAsync<T>(string key, int expires, [NotNull]Func<T> func)
        {
            if (await RedisHelper.ExistsAsync(key))
            {
                return await RedisHelper.GetAsync<T>(key);
            }
            else
            {
                var data = func.Invoke();
                await RedisHelper.SetAsync(key, data, expires);
                return data;
            }
        }

    }
}
