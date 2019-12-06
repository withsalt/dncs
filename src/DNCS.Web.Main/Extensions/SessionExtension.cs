using DNCS.LogInfo;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WithSalt.Common.Json;

namespace DNCS.Web.Main.Extensions
{
    public static class SessionExtension
    {
        #region Set

        /// <summary>
        /// 设置 类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<T>(this ISession session, string key, T value) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(key) || value == null)
                {
                    return;
                }
                string serializeValue = JsonHelper.SerializeObject(value);
                session.SetString(key, serializeValue);
            }
            catch (Exception ex)
            {
                Log.Error($"Set session by key({key}) failed. {ex.Message}");
            }
        }

        /// <summary>
        /// 设置 int
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(this ISession session, string key, int value)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }
                session.SetString(key, value.ToString());
            }
            catch (Exception ex)
            {
                Log.Error($"Set session by key({key}) failed. {ex.Message}");
            }
        }

        /// <summary>
        /// 设置 bool
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        #endregion

        #region Get

        public static T Get<T>(this ISession session, string key) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return default;
                }
                string result = session.GetString(key);
                if (string.IsNullOrEmpty(result))
                {
                    return default;
                }
                T resultObj = JsonHelper.DeserializeJsonToObject<T>(result);
                return resultObj;
            }
            catch (Exception ex)
            {
                Log.Error($"Get session by key({key}) failed. {ex.Message}");
                return default;
            }
        }

        public static int Get(this ISession session, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return default;
                }
                string result = session.GetString(key);
                if (string.IsNullOrEmpty(result))
                {
                    return default;
                }
                if (int.TryParse(result, out int num))
                {
                    return num;
                }
                else
                {
                    throw new Exception($"Get value is not number. key is {key}");
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Get session by key({key}) failed. {ex.Message}");
                return default;
            }
        }

        #endregion

        #region Clear

        public static void Clear(this ISession session, string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }
                session.Remove(key);
            }
            catch (Exception ex)
            {
                Log.Error($"Set null session by key({key}) failed. {ex.Message}");
            }
        }

        #endregion

    }
}
