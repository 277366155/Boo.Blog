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
        static string host = "https://so.gushiwen.cn";
        static Dictionary<string, string> TypeXpathDic = new Dictionary<string, string>();
        static GushiwenCrawler()
        {
            TypeXpathDic.Add("诗文", "//div[@class='left']/div[@class='sons']/div[@class='typecont']/span/a");
            TypeXpathDic.Add("作者", "//div[@class='left']/div[@class='sonspic']/div[@class='cont']/p/a[0]");
        }

        public static async Task StartAsync()
        {
            await LoadShiwenAsync();
        }


        #region 诗文页面爬取
        /// <summary>
        ///https://so.gushiwen.cn/shiwens/， 抓取页面右侧类型链接
        /// </summary>
        public static async Task<List<string>> LoadShiwenDepth1Async()
        {
            var url = "https://so.gushiwen.cn/shiwens/";
            var doc = await new HtmlWeb().LoadFromWebAsync(url);
            var currentTypeXpath = $"//div[@class='right']//div[@id='right1']/div[@class='cont']/a";

            var nodes = doc.DocumentNode.SelectNodes(currentTypeXpath);

            //Console.WriteLine("类目：" + doc.DocumentNode.SelectNodes(currentTypeXpath).Select(a => a.InnerText).ToJson());

            return nodes.Select(a => host + a.GetAttributeValue("href", "")).ToList();
        }
        /// <summary>
        /// 爬取诗文二级页面中的链接，如 https://so.gushiwen.cn/gushi/sanbai.aspx 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<List<KeyValuePair<string, string>>> LoadShiwenDepth2Async(string url)
        {
            var linkXpath = "//div[@class='left']//div[@class='sons']//span/a";
            var doc = await new HtmlWeb().LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes(linkXpath);
            return nodes.Select(a => KeyValuePair.Create(a.InnerText, host + a.GetAttributeValue("href", ""))).ToList();
        }

        /// <summary>
        /// 爬取作者，朝代，正文
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task LoadShiwenDepth3Async(string url)
        {
            //todo：判断url是否存在于列表，存在则不再爬取
            var doc = await new HtmlWeb().LoadFromWebAsync(url);
            var node = doc.DocumentNode;
            var authorXpath = "//div[@id='sonsyuanwen']//a[1]";
            var dynastyXpath = "//div[@id='sonsyuanwen']//a[2]";
            var contentXpath = "//div[@id='sonsyuanwen']//div[@class='contson']";
            
           var author= node.SelectSingleNode(authorXpath).InnerText;
            var dynasty = node.SelectSingleNode(dynastyXpath).InnerText;
            var content = node.SelectSingleNode(contentXpath).InnerText;

            //todo1：insert into dbtable
            //todo2：记录当前url到列表中
            Console.WriteLine(author+dynasty);
            Console.WriteLine(content);
        }

        public static async Task LoadShiwenAsync()
        {
            var list = await LoadShiwenDepth1Async();
            foreach (var li in list)
            {
                var aLinkInfos = await LoadShiwenDepth2Async(li);
                foreach (var info in aLinkInfos)
                {
                    await LoadShiwenDepth3Async(info.Value);
                }
            }
        }


        #endregion 诗文页面爬取

        /// <summary>
        /// 抓取页面中“略缩”链接
        /// </summary>
        /// <param name="url"></param>
        public static async Task<List<string>> LoadTypeLinkAsync(string url)
        {
            var urls = new string[] {
                "https://so.gushiwen.cn/guwen/",
                "https://so.gushiwen.cn/authors/"
            };
            var doc = await new HtmlWeb().LoadFromWebAsync(url);
            var currentTypeXpath = $"//div[@id='leftLuesuo']/div[@class='typecont']/span/a";

            var nodes = doc.DocumentNode.SelectNodes(currentTypeXpath);

            Console.WriteLine("类目：" + doc.DocumentNode.SelectNodes(currentTypeXpath).Select(a => a.InnerText).ToJson());

            return nodes.Select(a => host + a.GetAttributeValue("href", "")).ToList();
        }
    }
}
