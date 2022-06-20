using AbotSpider.Crawlers.Toutiao;
using Microsoft.Extensions.DependencyInjection;
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
            //var service = app.ServiceProvider.GetService<GushiwenCrawler>();
            //await service.StartAsync();
            var toutiaoService = app.ServiceProvider.GetService<ToutiaoCrawler>();
            await toutiaoService.StartAsync();
            Console.ReadLine();
        }



        #region dotnetspider
        #endregion
    }
}
