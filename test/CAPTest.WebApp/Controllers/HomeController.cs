using Boo.Blog.ToolKits.Extensions;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace CAPTest.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        IHttpClientFactory _httpClient;
        IConfiguration _config;

        public HomeController(IConfiguration config, IHttpClientFactory httpClient,ILogger<HomeController> logger)
        {
            _config = config;
            _httpClient = httpClient;
            _logger = logger;
        }


        [HttpGet("0/{code}")]
        public async Task<IActionResult> Get0(string code)
        {
            var client =  _httpClient.CreateClient();
            //
            //Host: bff.gds.org.cn
            //Connection: keep - alive
            //sec - ch - ua: " Not;A Brand"; v = "99", "Google Chrome"; v = "97", "Chromium"; v = "97"
            //Accept: application / json, text / plain, */*
            //sec-ch-ua-mobile: ?0
            //User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.99 Safari/537.36
            //sec-ch-ua-platform: "Windows"
            //Origin: https://www.gds.org.cn
            //Sec-Fetch-Site: same-site
            //Sec-Fetch-Mode: cors
            //Sec-Fetch-Dest: empty
            //Referer: https://www.gds.org.cn/
            //Accept-Encoding: gzip, deflate, br
            //Accept-Language: zh-CN,zh;q=0.9
            //
            client.DefaultRequestHeaders.Add("Origin"," https://www.gds.org.cn");
            client.DefaultRequestHeaders.Add("Referer","https://www.gds.org.cn/");

            var resp=await client.GetAsync("https://bff.gds.org.cn/gds/searching-api/ProductService/ProductListByGTIN?PageSize=30&PageIndex=1&SearchItem=0"+code);
            return Ok(resp);
        }

        /// <summary>
        /// ����·��Ϊ��test.show
        /// </summary>
        /// <param name="count"></param>
        /// <param name="capBus"></param>
        /// <returns></returns>
        [HttpGet("1/{count}")]
        public IActionResult Get1(int count, [FromServices]ICapPublisher capBus)
        {
            count = count <= 0 ? 1 : count;
            for (var i = 0; i < count; i++)
            {
                var header = new Dictionary<string, string>()
                {
                    ["my.header.first"] = "first_"+i,
                    ["my.header.second"] = "second_"+i
                };
                capBus.Publish("test.show",  $"ʵʱ������Ϣ i={i}����ǰʱ����{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", header);
            }
            return Ok();
        }

        /// <summary>
        ///����·��Ϊ��service.show
        /// </summary>
        /// <param name="capBus"></param>
        /// <returns></returns>
        [HttpGet("2")]
        public IActionResult Get2([FromServices] ICapPublisher capBus)
        {
            capBus.PublishDelay(TimeSpan.FromSeconds(10), "service.show", $"ʵʱ������Ϣ����ǰʱ����{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            return Ok();
        }

        //[NonAction]
        //[CapSubscribe("test.show")]
        //public void ReceiveMessage(string msg, [FromCap]CapHeader header)
        //{            
        //    Console.WriteLine($"[{DateTime.Now}]���յ���Ϣ��{msg}");
        //    if (header != null)
        //    {
        //        Console.WriteLine($"[{DateTime.Now}]���յ���Ϣͷ��{header.ToJson()}");
        //    }
        //}

        ///// <summary>
        ///// group��Ӧrabbitmq�е�queueName
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //[NonAction]
        //[CapSubscribe("service.show",  Group = "booCAP.ConsoleTest")]
        //public async Task CheckReceivedMessageAsync(string msg)
        //{
        //    await Console.Out.WriteLineAsync("ConsoleTest-" + msg);
        //    var rd = new Random();
        //    var val=rd.Next(100);
        //    if (val < 80)
        //    {
        //        throw new Exception($"ConsoleTest-��ǰrandom value = {val} , �����쳣������");
        //    }
        //}

        ///// <summary>
        ///// group��Ӧrabbitmq�е�queueName
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //[NonAction]
        //[CapSubscribe("service.show",  Group = "booCAP.WebTest")]
        //public async Task CheckReceivedMessage1Async(string msg)
        //{
        //    await Console.Out.WriteLineAsync("rec1_ConsoleTest-" + msg);

        //}
    }
}