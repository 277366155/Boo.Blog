using Boo.Blog.Application.HelloWorld;
using Boo.Blog.Consts;
using Boo.Blog.ToolKits.Cache;
using Boo.Blog.ToolKits.Configurations;
using CSRedis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Boo.Blog.HttpApi.Controllers
{
    /// <summary>
    /// ≤‚ ‘swaggerŒƒµµ
    /// </summary>
    [ApiExplorerSettings(GroupName = SwaggerGrouping.GroupNameV2)]
    public class HelloWorldController : ApiBaseController
    {
        readonly IHelloWorldService _helloWorldService;
        public HelloWorldController(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
        }

        /// <summary>
        /// ≤‚ ‘Ω”ø⁄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string HelloWorld()
        {
            return _helloWorldService.HelloWorld();
        }
        /// <summary>
        /// ª∫¥Ê≤‚ ‘
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("cacheSet")]
        public bool CacheSet(string key, string value)
        {
            return _helloWorldService.CacheTest(key, value);
        }
    }
}