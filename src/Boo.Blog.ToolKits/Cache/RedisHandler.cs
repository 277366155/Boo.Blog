using CSRedis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace Boo.Blog.ToolKits.Cache
{
    public class RedisHandler: IRedisHandler
    {
        private Dictionary<RedisType, CSRedisClient> _redisClientDic = new Dictionary<RedisType, CSRedisClient>();
        static object _locker = new object();
        public RedisHandler(IEnumerable<RedisHandlerOption> redisHandlerOptionList)
        {
            foreach (var opt in redisHandlerOptionList)
            {
                if (!_redisClientDic.ContainsKey(opt.RedisType))
                {
                    lock (_locker)
                    {
                        if (!_redisClientDic.ContainsKey(opt.RedisType))
                        {
                            _redisClientDic.Add(opt.RedisType, InitRedisClient(opt));
                        }
                    }
                }
            }
        }

        private CSRedisClient InitRedisClient(RedisHandlerOption option)
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
            return client;
        }
        public  CSRedisClient GetRedisClient(RedisType redisType)
        {
            return _redisClientDic.GetValueOrDefault(redisType);
        }

        #region string缓存
        public async Task<string> GetAsync(string key, RedisType redisType = RedisType.Default)
        {
            return await _redisClientDic.GetValueOrDefault(redisType).GetAsync(key);
        }
        public async Task<T> GetAsync<T>(string key, RedisType redisType = RedisType.Default)
        {
            return await _redisClientDic.GetValueOrDefault(redisType).GetAsync<T>(key);
        }
        public async Task<bool> SetAsync(string key,object value, TimeSpan timeSpan, RedisType redisType= RedisType.Default )
        {
            return await _redisClientDic.GetValueOrDefault(redisType).SetAsync(key,value,timeSpan);
        }
        public async Task<bool> SetAsync(string key, object value, int expireSecond=-1, RedisType redisType = RedisType.Default)
        {
            return await _redisClientDic.GetValueOrDefault(redisType).SetAsync(key,value,expireSecond);
        }        
        #endregion string缓存
        /// <summary>
        /// 尝试从缓存查询数据，若缓存没有，就执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="expires">过期时间，单位秒</param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<T> TryToGetFromCacheAsync<T>(string key, int expires, [NotNull] Func<T> func)
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