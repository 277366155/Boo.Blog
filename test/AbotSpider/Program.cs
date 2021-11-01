using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using AbotSpider.Service;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AbotSpider
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .WriteTo.Console()
            //    .CreateLogger();

            //Log.Logger.Information("Demo starting up!");

            ////await DemoSimpleCrawler();
            //await DemoSinglePageRequest();
            GushiwenCrawler.LoadHtml();
            Console.ReadLine();
        }

        private static async Task DemoSimpleCrawler()
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 10, //Only crawl 10 pages
                MinCrawlDelayPerDomainMilliSeconds = 3000 //Wait this many millisecs between requests
            };
            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted;//Several events available...

            var crawlResult = await crawler.CrawlAsync(new Uri("https://www.gushiwen.cn"));
        }

        private static async Task DemoSinglePageRequest()
        {
            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());

            var crawledPage = await pageRequester.MakeRequestAsync(new Uri("https://www.gushiwen.cn"));
            Log.Logger.Information("{result}", new
            {
                url = crawledPage.Uri,
                status = Convert.ToInt32(crawledPage.HttpResponseMessage.StatusCode)
            });
        }

        private static void PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var httpStatus = e.CrawledPage.HttpResponseMessage.StatusCode;
            var rawPageText = e.CrawledPage.Content.Text;
        }
    }
}
