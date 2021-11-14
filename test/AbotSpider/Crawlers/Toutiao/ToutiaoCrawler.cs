using HtmlAgilityPack;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Boo.Blog.ToolKits.Extensions;
using System;

namespace AbotSpider.Crawlers.Toutiao
{
    public class ToutiaoCrawler : ITransientDependency
    {
        static string homeUrl = "https://www.toutiao.com/";
        static string hotBoardUrl = "https://www.toutiao.com/hot-event/hot-board/?origin=toutiao_pc";

        public async Task StartAsync()
        {
            await GetHotBoardInfoAsync(hotBoardUrl);
        }
        public async Task GetHotBoardInfoAsync(string hotUrl)
        {
            var doc = await new HtmlWeb().LoadFromWebAsync(hotUrl);
            doc.Text = doc.Text.Replace("\u0026", "&");
            var response = doc.Text.ToObj<Rootobject>();

            if (!response.status.Equals("success"))
            {
                return;
            }
            foreach (var data in response.data)
            {
                //等待3s防止触发反爬警告
                Task.Delay(500).Wait();
                await GetTrendingPageAsync(data.Url);
                //Console.WriteLine($"标题：{data.Title}");
                //Console.WriteLine($"URL：{data.Url}");
            }
            Console.WriteLine($"总共{response.data.Length}条数据");
        }
        public async Task GetTrendingPageAsync(string url)
        {            
            var htmlWeb = new HtmlWeb() ;
            var doc = await htmlWeb.LoadFromWebAsync(url);

            var linkNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-title article-column-single-title']/a");
            var jsonData = doc.DocumentNode.SelectSingleNode("//script[@id='RENDER_DATA']");
            Console.WriteLine(jsonData.InnerText);
            return;

            if (linkNode == null || linkNode.GetAttributeValue("href", "").IsNullOrWhiteSpace())
            {
                return;
            }
            var page = await htmlWeb.LoadFromWebAsync(linkNode.GetAttributeValue("href", ""));
            var titleNode = page.DocumentNode.SelectSingleNode("//div[@class='article-content']/h1");
            var title = titleNode.InnerText;
            var timeNode = page.DocumentNode.SelectSingleNode("//div[@class='article-meta']/span");
            var time = titleNode.InnerText;
            var mediaNode = page.DocumentNode.SelectSingleNode("//div[@class='article-meta']/span[0]");
            var media = mediaNode.InnerText;
            var articleNode = page.DocumentNode.SelectSingleNode("//article[@class='syl-article-base tt-article-content syl-page-article syl-device-pc']/p");
            var content = articleNode.InnerText;
            Console.ForegroundColor = (ConsoleColor)new Random().Next(1, 16);
            Console.WriteLine($"标题：{title}\r时间：{time}，{media}\r正文：{content}");
            Console.ResetColor();
        }
    }
}
