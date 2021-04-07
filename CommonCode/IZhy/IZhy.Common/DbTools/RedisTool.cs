using IZhy.Common.BasicTools;
using IZhy.Common.Const;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IZhy.Common.DbTools
{
    /// <summary>
    /// Redis 操作 工具类
    /// </summary>
    public static class RedisTool
    {
        /// <summary>
        /// 是否启用 Redis
        /// </summary>
        public static bool IsEnableRedis()
        {
            try
            {
                return Convert.ToBoolean(CommonConfigTool.GetConfig(CommonConfigFieldsConst.IsEnableRedis) ?? false);
            }
            catch (Exception)
            {
                return false;
            }
        }


        static RedisTool()
        {
            if (!IsEnableRedis())
            {
                return;
            }
            RedisHelper.Initialization(new CSRedis.CSRedisClient(Convert.ToString(CommonConfigTool.GetConfig(CommonConfigFieldsConst.RedisConnString))));
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
        /// 获取 redis 里键对应的值
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            if (!IsEnableRedis())
            {
                return default(T);
            }
            return RedisHelper.Get<T>(key);
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
        public static Task<T> GetAsync<T>(string key)
        {
            if (!IsEnableRedis())
            {
                return Task.FromResult<T>(default(T));
            }
            return RedisHelper.GetAsync<T>(key);
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
    }
}
