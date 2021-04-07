using IZhy.Common.SimpleTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IZhy.Common.BasicTools
{
    /// <summary>
    /// json 处理 工具类
    /// </summary>
    public static class JsonTool
    {
        /// <summary>
        /// 设置 json 序列化格式
        /// </summary>
        /// <param name="isFormatJson">是否格式化 json 字符串，默认 false</param>
        /// <param name="isIgnoreNull">是否忽略 null 值，默认 false</param>
        /// <param name="dtmFormat">日期格式化参数，默认 "yyyy-MM-dd HH:mm:ss"</param>
        /// <returns></returns>
        private static JsonSerializerSettings SetJsonSerializerFormat(bool isFormatJson = false,
                                                                      bool isIgnoreNull = false,
                                                                      string dtmFormat = null)
        {
            JsonSerializerSettings jss = new JsonSerializerSettings();
            IList<JsonConverter> jcList = new List<JsonConverter>();

            //日期时间格式化处理
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = string.IsNullOrWhiteSpace(dtmFormat) ? "yyyy-MM-dd HH:mm:ss" : dtmFormat //日期时间格式化处理
            };
            jcList.Add(timeConverter);

            jss.Converters = jcList;
            if (isFormatJson)
            {
                jss.Formatting = Formatting.Indented; //格式化Json字符串    
            }
            if (isIgnoreNull)
            {
                jss.NullValueHandling = NullValueHandling.Ignore; //忽略null值    
            }

            return jss;
        }


        /// <summary>
        /// object 对象转换为 json 格式的字符串
        /// </summary>
        /// <param name="obj">可序列化对象</param>
        /// <param name="isFormatJson">是否格式化 Json 字符串，默认 false</param>
        /// <param name="isIgnoreNull">是否忽略 null 值，默认 false</param>
        /// <param name="dtmFormat">日期格式化参数，默认 "yyyy-MM-dd HH:mm:ss"</param>
        /// <returns></returns>
        public static string ObjectToJson(object obj,
                                          bool isFormatJson = false,
                                          bool isIgnoreNull = false,
                                          string dtmFormat = null)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            if (obj is string)
            {
                return obj as string;
            }

            return JsonConvert.SerializeObject(obj, SetJsonSerializerFormat(isFormatJson, isIgnoreNull, dtmFormat));
        }

        /// <summary>
        /// json 格式的字符串 转换为 T 对象
        /// </summary>
        /// <typeparam name="T">泛型 类型</typeparam>
        /// <param name="json">json 格式的字符串</param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// json 格式的字符串 转换为 object 对象
        /// </summary>
        /// <param name="json">json 格式的字符串</param>
        /// <returns></returns>
        public static object JsonToObject(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// json 格式的字符串 转换为 dynamic 匿名对象
        /// </summary>
        /// <param name="json">json 格式的字符串</param>
        /// <returns></returns>
        public static dynamic JsonToDynamic(string json)
        {
            dynamic dyData = JsonToObject(json);
            return dyData;
        }


        /// <summary>
        /// 读取 json 内容的文件，并转化为 T 对象
        /// </summary>
        /// <typeparam name="T">泛型 类型</typeparam>
        /// <param name="path">json 文件的绝对路径</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static T JsonFileToObject<T>(string path, Encoding encoding = null)
        {
            string jsonStr = FilesTool.ReadFileToAll(path, encoding);
            return JsonToObject<T>(jsonStr);
        }

        /// <summary>
        /// 读取 json 内容的文件，并转化为 dynamic 匿名对象
        /// </summary>
        /// <param name="path">json 文件的绝对路径</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static dynamic JsonFileToDynamic(string path, Encoding encoding = null)
        {
            string jsonStr = FilesTool.ReadFileToAll(path, encoding);
            return JsonToDynamic(jsonStr);
        }

        /// <summary>
        /// object 对象转换为 json 内容的文件
        /// </summary>
        /// <param name="obj">object 对象</param>
        /// <param name="path">json 文件的绝对路径（建议文件格式用 .json 后缀）</param>
        /// <param name="encoding">字符编码，默认 UTF-8</param>
        /// <returns></returns>
        public static bool ObjectToJsonFile(object obj, string path, Encoding encoding = null)
        {
            if (obj == null || string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            string jsonStr = ObjectToJson(obj, true);

            if (string.IsNullOrWhiteSpace(jsonStr))
            {
                return false;
            }
            else
            {
                return FilesTool.WriteFileCreate(path, jsonStr, encoding);
            }
        }


        /// <summary>
        /// JToken 转换为 T 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jToken"></param>
        /// <returns></returns>
        public static T JTokenToObject<T>(JToken jToken)
        {
            if (jToken == null)
            {
                return default(T);
            }

            try
            {
                return jToken.ToObject<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Newtonsoft.Json.Linq.JArray 转换成 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objJArray"></param>
        /// <returns></returns>
        public static List<T> ObjJArrayToList<T>(object objJArray)
        {
            if (objJArray == null)
            {
                return null;
            }

            if (!(objJArray is JArray jArray))
            {
                return null;
            }

            List<T> list = new List<T>();
            if (jArray.Count > 0)
            {
                jArray.ToList().ForEach(j =>
                {
                    list.Add(JTokenToObject<T>(j));
                });
            }
            return list;
        }

        /// <summary>
        /// Newtonsoft.Json.Linq.JArray 转换成 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jArray"></param>
        /// <returns></returns>
        public static List<T> JArrayToList<T>(JArray jArray)
        {
            return ObjJArrayToList<T>(jArray);
        }
    }
}
