using Boo.Blog.ToolKits.Extensions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbotSpider.Service
{
    public class GushiwenCrawler
    {
        static string host = "https://so.gushiwen.cn/";
        static Dictionary<string,string> TypeXpathDic= new Dictionary<string,string>();
        static GushiwenCrawler()
        {
            TypeXpathDic.Add("诗文", "//div[@class='left']/div[@class='sons']/div[@class='typecont']/span/a");
            TypeXpathDic.Add("作者", "//div[@class='left']/div[@class='sonspic']/div[@class='cont']/p/a[0]");
        }

        public static async Task StartAsync()
        {
            var kvs = await GetMoreLinkAsync();
            if (kvs == null)
            {
                return;
            }
            foreach (var kv in kvs)
            {
                var typeLinks=await LoadTypeLinkAsync(kv.Value);
                foreach (var link in typeLinks)
                {
                    Console.WriteLine(link);
                }
            }
        }

        /// <summary>
        /// 获取首页右侧所有的“更多”的类型名称与链接地址
        /// </summary>
        /// <returns></returns>
        public static async Task<Dictionary<string,string>> GetMoreLinkAsync()
        {
            string homeUrl = "https://www.gushiwen.cn/";
            var doc = await  new HtmlWeb().LoadFromWebAsync(homeUrl);
            var xpath = "//div[@class='right']//div[@class='sons']//div[@class='cont']/a";
            var node = doc.DocumentNode.SelectNodes(xpath);
            var linkList = node.Where(a => a.GetAttributeValue("target", "") == "").Select(a => a.GetAttributeValue("href", ""))?.ToList();

            var titleXpath = "//div[@class='right']//div[@class='sons']/div[@class='title']";
            var node2=doc.DocumentNode.SelectNodes(titleXpath);
            var titleList = node.Where(a => a.GetAttributeValue("target", "") == "").Select(a=>a.InnerText).ToList();

            var result = new Dictionary<string,string>();
            for (var i = 0; i < linkList.Count; i++)
            {
                result.Add(titleList[i],linkList[i]);
            }
            return result;
        }

        /// <summary>
        /// 在各个“更多”子页面抓取右侧所有类型链接
        /// </summary>
        /// <param name="url"></param>
        public static async Task< List<string>> LoadTypeLinkAsync(string url)
        {
            var doc =await  new HtmlWeb().LoadFromWebAsync(url);  
            var currentTypeXpath = $"//div[@class='right']//div[@class='sons']/div[@class='cont']/a";

            var nodes = doc.DocumentNode.SelectNodes(currentTypeXpath);

            Console.WriteLine("类目：" + doc.DocumentNode.SelectNodes(currentTypeXpath).Select(a => a.InnerText).ToJson());
            
            return nodes.Select(a=> host + a.GetAttributeValue("href", "")).ToList();
        }
    }
}
