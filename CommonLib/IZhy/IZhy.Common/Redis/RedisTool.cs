using IZhy.Common.BasicTools;
using IZhy.Common.Const;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IZhy.Common.Redis
{
    /// <summary>
    /// Redis 操作 工具类
    /// </summary>
    public static class RedisTool
    {
        private static string _redisConn;

        static RedisTool()
        {
            if (!IsEnableRedis())
            {
                return;
            }
        }


        /// <summary>
        /// 是否启用 Redis
        /// </summary>
        public static bool IsEnableRedis()
        {
            try
            {
                bool isEnable = Convert.ToBoolean(CommonConfigTool.GetConfig(CommonConfigFieldsConst.IsEnableRedis) ?? false);

                string redisConn = Convert.ToString(CommonConfigTool.GetConfig(CommonConfigFieldsConst.RedisConnString));
                if (isEnable && _redisConn != redisConn)
                {
                    _redisConn = redisConn;
                    RedisHelper.Initialization(new CSRedis.CSRedisClient(_redisConn));
                }

                return isEnable;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// 向 redis 里设置键值
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">键对应的值</param>
        /// <param name="expireSeconds">过期时间（单位 s 秒），默认 -1，永不过期</param>
        /// <returns></returns>
        public static bool Set(string key, object value, int expireSeconds = -1)
        {
            if (!IsEnableRedis())
            {
                return false;
            }
            return RedisHelper.Set(key, value, expireSeconds);
        }

        /// <summary>
        /// 向 redis 里设置键值
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">键对应的值</param>
        /// <param name="expireSeconds">过期时间（单位 s 秒），默认 -1，永不过期</param>
        /// <returns></returns>
        public static Task<bool> SetAsync(string key, object value, int expireSeconds = -1)
        {
            if (!IsEnableRedis())
            {
                return Task.FromResult<bool>(false);
            }
            return RedisHelper.SetAsync(key, value, expireSeconds);
        }


        /// <summary>
        /// 获取 redis 里键对应的值
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (!IsEnableRedis())
            {
                return default;
            }
            return RedisHelper.Get<T>(key);
        }

        /// <summary>
        /// 获取 redis 里键对应的值
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static Task<T> GetAsync<T>(string key)
        {
            if (!IsEnableRedis())
            {
                return Task.FromResult<T>(default);
            }
            return RedisHelper.GetAsync<T>(key);
        }

        /// <summary>
        /// 获取 redis 里键对应的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string Get(string key)
        {
            if (!IsEnableRedis())
            {
                return null;
            }
            return RedisHelper.Get(key);
        }

        /// <summary>
        /// 获取 redis 里键对应的值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static Task<string> GetAsync(string key)
        {
            if (!IsEnableRedis())
            {
                return Task.FromResult<string>(null);
            }
            return RedisHelper.GetAsync(key);
        }


        private static TimeSpan KeyTimeSpan(long milliseconds)
        {
            // -1 表示 key 永不过期
            // -2 表示 key 不存在
            if (milliseconds == -2)
            {
                return new TimeSpan(0);
            }
            TimeSpan span = TimeSpan.FromMilliseconds(milliseconds);
            return span;
        }

        /// <summary>
        /// 获取 Key 的有效时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<TimeSpan> KeyValidTimeAsync(string key)
        {
            if (!IsEnableRedis())
            {
                return new TimeSpan(0);
            }
            long ms = await RedisHelper.PTtlAsync(key);
            return KeyTimeSpan(ms);
        }

        /// <summary>
        /// 获取 Key 的有效时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TimeSpan KeyValidTime(string key)
        {
            if (!IsEnableRedis())
            {
                return new TimeSpan(0);
            }
            long ms = RedisHelper.PTtl(key);
            return KeyTimeSpan(ms);
        }


        /// <summary>
        /// 更新 Key 的过期时间，单位 秒
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool Expire(string key, int seconds)
        {
            if (!IsEnableRedis())
            {
                return false;
            }
            return RedisHelper.Expire(key, seconds);
        }

        /// <summary>
        /// 更新 Key 的过期时间，单位 秒
        /// </summary>
        /// <param name="key"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static async Task<bool> ExpireAsync(string key, int seconds)
        {
            if (!IsEnableRedis())
            {
                return false;
            }
            return await RedisHelper.ExpireAsync(key, seconds);
        }
    }
}
