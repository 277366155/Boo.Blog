using Boo.Blog.ToolKits.Extensions;
using HtmlAgilityPack;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace AbotSpider.Crawlers
{
    public class GushiwenCrawler : ITransientDependency
    {
        static string host = "https://so.gushiwen.cn";
        static string urlMemberKey = "CrawlerUrl";
        static int crawCount = 0;
        static object lockObj = new object();
        int GetCrawCount()
        {
            lock (lockObj)
            {
                return ++crawCount;
            }
        }
        MongoDbPoemRepository _poemRepository;

        public GushiwenCrawler(MongoDbPoemRepository poemRepository)
        {
            _poemRepository = poemRepository;
        }
        public async Task StartAsync()
        {
            await LoadShiwenAsync();
        }


        #region 诗文页面爬取
        /// <summary>
        ///https://so.gushiwen.cn/shiwens/， 抓取页面右侧类型链接
        /// </summary>
        public async Task<List<string>> LoadShiwenDepth1Async()
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
        public async Task<List<KeyValuePair<string, string>>> LoadShiwenDepth2Async(string url)
        {
            var linkXpath = "//div[@class='left']//div[@class='sons']//span/a";
            var doc = await new HtmlWeb().LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes(linkXpath);
            
            return nodes.Where(a=>!string.IsNullOrWhiteSpace(a.GetAttributeValue("href",""))).Select(a => KeyValuePair.Create(a.InnerText, a.GetAttributeValue("href", "").Contains("https://")? a.GetAttributeValue("href", "") : host + a.GetAttributeValue("href", ""))).ToList();
        }

        /// <summary>
        /// 爬取作者，朝代，正文
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task LoadShiwenDepth3Async(string title, string url)
        {
            //判断url是否存在于列表，存在则不再爬取
            if (await RedisHelper.SIsMemberAsync(urlMemberKey, url))
            {
                Console.WriteLine($"跳过:"+GetCrawCount());
                return;
            }

            var doc = await new HtmlWeb().LoadFromWebAsync(url);
            var node = doc.DocumentNode;
            var authorXpath = "//div[@id='sonsyuanwen']//a[1]";
            var dynastyXpath = "//div[@id='sonsyuanwen']//a[2]";
            var contentXpath = "//div[@id='sonsyuanwen']//div[@class='contson']";
            if (node.SelectSingleNode(authorXpath) == null)
            {
                return;
            }
            var author = node.SelectSingleNode(authorXpath).InnerText;
            var dynasty = node.SelectSingleNode(dynastyXpath).InnerText.Replace("〔", "").Replace("〕", "");
            var content = node.SelectSingleNode(contentXpath).InnerText.Replace("\\n", "");

            //1：insert into mongoDb
            var insertResult = await _poemRepository.InsertAsync(new Poem(Guid.NewGuid()) { Author = author, Content = content, Dynasty = dynasty, Title = title, Tags = new string[] { author, dynasty } });
            if (insertResult != null)
            {
                //2：记录当前url到列表中
                RedisHelper.SAdd(urlMemberKey, url);
            }
            Console.WriteLine(author + dynasty + " is ok ");
        }

        public async Task LoadShiwenAsync()
        {
            var list = await LoadShiwenDepth1Async();
            foreach (var li in list)
            {
                var aLinkInfos = await LoadShiwenDepth2Async(li);
                foreach (var info in aLinkInfos)
                {

                    await LoadShiwenDepth3Async(info.Key, info.Value);
                }
            }
        }

        #endregion 诗文页面爬取

        /// <summary>
        /// 抓取页面中“略缩”链接
        /// </summary>
        /// <param name="url"></param>
        public async Task<List<string>> LoadTypeLinkAsync(string url)
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
