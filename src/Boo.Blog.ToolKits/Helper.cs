using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.ToolKits
{
    public static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="perfix"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateSerialNumber(this int num, int length, string perfix)
        {
            if (num <= 0)
                return "";
            return perfix + num.ToString().PadLeft(length, '0');
        }

        public static Dictionary<string, Tuple<int, List<string>>> GenerateDetail()
        {
            //key：perfix , value：item1-currentNum，item2-Lenth，item3-生成流水号数量
            var settings = new Dictionary<string, Tuple<int, int, int>>
            {
                { "SD", new Tuple<int, int, int>(0, 8,3) },
                { "KJD",  new Tuple< int, int, int >(2, 10,6) },
                { "KNM", new Tuple< int, int, int >(21, 9,2)  }
            };
            var result = new Dictionary<string, Tuple<int, List<string>>>();
            
            foreach (var s in settings)
            {
                //当前编码
                var currentNum = s.Value.Item1;
                var serialNumberList = new List<string>();
                for (var i = 0; i < s.Value.Item3; i++)
                {
                    ++currentNum;
                    serialNumberList.Add(currentNum.GenerateSerialNumber(s.Value.Item2, s.Key));
                }
                result.Add(s.Key, new Tuple<int, List<string>>(currentNum, serialNumberList));
            }
            return result;
        }
    }
}
