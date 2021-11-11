using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using AbotSpider.Crawlers;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace AbotSpider
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using var app = AbpApplicationFactory.Create<AbotSipderModule>(opt => opt.UseAutofac());
            app.Initialize();
            var service = app.ServiceProvider.GetService<GushiwenCrawler>();
            await service.StartAsync();

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
