using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using Boo.Blog.ToolKits.Extensions;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbotSpider.Service
{
    public class GushiwenCrawler
    {
        static string url = "https://www.gushiwen.cn";
        public static void LoadHtml()
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            ///html/body/div[2]/div[2]/div[2]/div[2]
            var xpathFilter = "//div[@class='right']//div[@class='sons']"; 
            var nodes= doc.DocumentNode.SelectNodes(xpathFilter);
            for (var i=0;i<nodes.Count;i++)
            {
                Console.WriteLine("****************************************************");
                var currentTitleXpath = $"{xpathFilter}/div[@class='title']";
                Console.WriteLine("分类："+doc.DocumentNode.SelectNodes(currentTitleXpath)[i].InnerText);
                Console.WriteLine("****************************************************");
                var currentTypeXpath = $"{xpathFilter}/div[@class='cont']/a[@target='_blank']";

                Console.WriteLine("类目："+ doc.DocumentNode.SelectNodes(currentTypeXpath).Select(a=>a.InnerText).ToJson());

            }
        }
    }
}
