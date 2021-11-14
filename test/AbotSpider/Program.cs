using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;
using AbotSpider.Crawlers;
using AbotSpider.Crawlers.Toutiao;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Threading;
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
            //var service = app.ServiceProvider.GetService<GushiwenCrawler>();
            //await service.StartAsync();
            //var toutiaoService = app.ServiceProvider.GetService<ToutiaoCrawler>();
            //await toutiaoService.StartAsync();
            Console.ReadLine();
        }



        #region abot
        private static async Task DemoSimpleCrawler()
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 10, //Only crawl 10 pages
                MinCrawlDelayPerDomainMilliSeconds = 3000 //Wait this many millisecs between requests
            };
            var crawler = new PoliteWebCrawler(config);

            crawler.PageCrawlCompleted += PageCrawlCompleted;//Several events available...

            var crawlResult = await crawler.CrawlAsync(new Uri("https://www.toutiao.com/amos_land_page/?category_name=topic_innerflow&event_type=hot_board&log_pb=%7B%22category_name%22%3A%22topic_innerflow%22%2C%22cluster_type%22%3A%222%22%2C%22enter_from%22%3A%22click_category%22%2C%22entrance_hotspot%22%3A%22outside%22%2C%22event_type%22%3A%22hot_board%22%2C%22hot_board_cluster_id%22%3A%227029111881872703525%22%2C%22hot_board_impr_id%22%3A%2220211114164152010212148044520984E3%22%2C%22jump_page%22%3A%22hot_board_page%22%2C%22location%22%3A%22news_hot_card%22%2C%22page_location%22%3A%22hot_board_page%22%2C%22rank%22%3A%221%22%2C%22source%22%3A%22trending_tab%22%2C%22style_id%22%3A%2240132%22%2C%22title%22%3A%22%E5%8D%81%E4%B9%9D%E5%B1%8A%E5%85%AD%E4%B8%AD%E5%85%A8%E4%BC%9A%E5%85%AC%E6%8A%A5%E5%8F%91%E5%B8%83%22%7D&rank=1&style_id=40132&topic_id=7029111881872703525"));
        }

        private static async Task DemoSinglePageRequest()
        {
            var pageRequester = new PageRequester(new CrawlConfiguration(), new WebContentExtractor());

            var crawledPage = await pageRequester.MakeRequestAsync(new Uri("https://www.toutiao.com/amos_land_page/?category_name=topic_innerflow&event_type=hot_board&log_pb=%7B%22category_name%22%3A%22topic_innerflow%22%2C%22cluster_type%22%3A%222%22%2C%22enter_from%22%3A%22click_category%22%2C%22entrance_hotspot%22%3A%22outside%22%2C%22event_type%22%3A%22hot_board%22%2C%22hot_board_cluster_id%22%3A%227029111881872703525%22%2C%22hot_board_impr_id%22%3A%2220211114164152010212148044520984E3%22%2C%22jump_page%22%3A%22hot_board_page%22%2C%22location%22%3A%22news_hot_card%22%2C%22page_location%22%3A%22hot_board_page%22%2C%22rank%22%3A%221%22%2C%22source%22%3A%22trending_tab%22%2C%22style_id%22%3A%2240132%22%2C%22title%22%3A%22%E5%8D%81%E4%B9%9D%E5%B1%8A%E5%85%AD%E4%B8%AD%E5%85%A8%E4%BC%9A%E5%85%AC%E6%8A%A5%E5%8F%91%E5%B8%83%22%7D&rank=1&style_id=40132&topic_id=7029111881872703525"));
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
        #endregion
    }
}
