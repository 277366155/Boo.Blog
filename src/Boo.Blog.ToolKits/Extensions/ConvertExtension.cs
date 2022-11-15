using System.IO;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace Boo.Blog.ToolKits.Extensions
{
    /// <summary>
    /// 类型转换拓展类
    /// </summary>
    public static class ConvertExtension
    {
        #region json与类型互转
        /// <summary>
        /// 将对象转为json字符串
        /// </summary>
        /// <param name="obj">对象模型</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) where T : class
        {
            if (obj != null)
            {
                return JsonSerializer.Serialize(obj);
            }

            return "";
        }

        /// <summary>
        /// json字符串转为对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public static T ToObj<T>(this string json) where T : class
        {
            if (!string.IsNullOrWhiteSpace(json))
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            return default(T);
        }
        #endregion

        #region xml转类型
        /// <summary>
        /// xml转为Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static T XMLToModel<T>(this string xml, Encoding encoding = null)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                return default(T);
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(xml)))
            {
                using (StreamReader streamReader = new StreamReader(ms, encoding))
                {
                    return (T)xmlSerializer.Deserialize(streamReader);
                }
            }
        }

        #endregion

        #region 字符串与字节序列互转
        /// <summary>
        /// 字符串序列转为字节序列
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] SerializeUtf8(this string str)
        {
            return str == null ? null : Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 字节序列转为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string DeserializeUtf8(this byte[] bytes)
        {
            return bytes == null ? null : Encoding.UTF8.GetString(bytes);
        }
        #endregion

        #region 字符串转数字类型（int,long,float,double,decimal）
        public static int TryToInt(this string str, int defaultNum = 0)
        {
            var num = defaultNum;
            int.TryParse(str, out num);
            return num;
        }
        public static long TryToLong(this string str, long defaultNum = 0)
        {
            var num = defaultNum;
            long.TryParse(str, out num);
            return num;
        }
        public static float TryToFloat(this string str, float defaultNum = 0)
        {
            var num = defaultNum;
            float.TryParse(str, out num);
            return num;
        }
        public static double TryToDouble(this string str, double defaultNum = 0)
        {
            var num = defaultNum;
            double.TryParse(str, out num);
            return num;
        }
        public static decimal TryToDecimal(this string str, decimal defaultNum = 0)
        {
            var num = defaultNum;
            decimal.TryParse(str, out num);
            return num;
        }
        #endregion
    }
}
