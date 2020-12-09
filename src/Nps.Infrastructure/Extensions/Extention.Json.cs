using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;

namespace Nps.Infrastructure.Extensions
{
    public static partial class Extention
    {
        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="isNullValue">序列化时是否去掉空值，默认不去掉</param>
        /// <returns>返回Json字符串</returns>
        public static string ToJson(this object obj, bool isNullValue = false)
        {
            var serializerSettings = new JsonSerializerSettings();
            if (isNullValue)
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;

            //Formatting.None会跳过不必要的空格和换行符
            //Formatting.Indented生成良好的显示格式,可读性更好。
            return JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns>返回T</returns>
        public static T FromJson<T>(this string jsonStr) => jsonStr.IsNullOrWhiteSpace() ? default : JsonConvert.DeserializeObject<T>(jsonStr);

        /// <summary>
        /// 将Json字符串转为List'T'
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns>返回List'T'</returns>
        public static List<T> ToList<T>(this string jsonStr) => jsonStr.IsNullOrWhiteSpace() ? null : JsonConvert.DeserializeObject<List<T>>(jsonStr);

        /// <summary>
        /// 将Json字符串转换为DataTable对象
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns>返回DataTable对象</returns>
        public static DataTable ToTable(this string jsonStr) => jsonStr.IsNullOrWhiteSpace() ? null : JsonConvert.DeserializeObject<DataTable>(jsonStr);

        /// <summary>
        /// 将Json字符串转换为JObject对象
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns>返回JObject对象</returns>
        public static JObject ToJObject(this string jsonStr) => jsonStr.IsNullOrWhiteSpace() ? JObject.Parse("{}") : JObject.Parse(jsonStr.Replace("&nbsp;", ""));
    }
}