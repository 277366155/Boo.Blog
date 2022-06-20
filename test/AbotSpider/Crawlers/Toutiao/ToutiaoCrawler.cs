using HtmlAgilityPack;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Boo.Blog.ToolKits.Extensions;
using System;
using PuppeteerSharp;

namespace AbotSpider.Crawlers.Toutiao
{
    public class ToutiaoCrawler : ITransientDependency
    {
        static string homeUrl = "https://www.toutiao.com/";
        static string hotBoardUrl = "https://www.toutiao.com/hot-event/hot-board/?origin=toutiao_pc";

        static ToutiaoCrawler()
        {
            new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision).Wait();
        }
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
            var html = await GetPageHtmlAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var linkNode0 = doc.DocumentNode.SelectSingleNode("//div[@class='article-title article-column-single-title']/a");
            //xpath从1计数
            var linkNode1 = doc.DocumentNode.SelectSingleNode("//div[@class='article-title ']/a[1]");
            string page = null;
            if (linkNode0 != null && !linkNode0.GetAttributeValue("href", "").IsNullOrWhiteSpace())
            {
                page = await GetPageHtmlAsync(linkNode0.GetAttributeValue("href", ""));
            }
            else if (linkNode1 != null && !linkNode1.GetAttributeValue("href", "").IsNullOrWhiteSpace())
            {
                page = await GetPageHtmlAsync(linkNode1.GetAttributeValue("href", ""));
            }
            else
            {
                Console.WriteLine($"xpath error : {url}");
                return;
            }
            doc.LoadHtml(page);

            var titleNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-content']/h1[1]");
            var title = titleNode?.InnerText;
            var timeNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-meta']/span[1]");
            var time = timeNode.InnerText;
            var mediaNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-meta']/span[@class='name']");
            var media = mediaNode.InnerText;
            var articleNode = doc.DocumentNode.SelectSingleNode("//article[@class='syl-article-base tt-article-content syl-page-article syl-device-pc']");
            var content = articleNode.InnerHtml;
            Console.ForegroundColor = (ConsoleColor)new Random().Next(1, 16);
            Console.WriteLine($"标题：{title}\r\n时间：{time}，{media}\r\n正文：{content}");
            Console.ResetColor();
        }

        public async Task<string> GetPageHtmlAsync(string url)
        {          
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false,
                Args = new string[] { "--no-sandbox" }                
            });
            var userAgent=await browser.GetUserAgentAsync();
            using var page = await browser.NewPageAsync();
            await page.GoToAsync(url, WaitUntilNavigation.Networkidle0);
            var content = await page.GetContentAsync();
            return content;
        }
    }
}
