using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Consts;
using Boo.Blog.HelloWorld;
using Boo.Blog.Middleware.Attributes;
using Boo.Blog.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Boo.Blog.HttpApi.Controllers
{
    /// <summary>
    /// ≤‚ ‘swaggerŒƒµµ
    /// </summary>
    [IgnoreAuthentication]
    [ApiExplorerSettings(GroupName = SwaggerGrouping.GroupNameV2)]
    public class HelloWorldController : ApiBaseController
    {
        readonly IHelloWorldService _helloWorldService;
        readonly IBlogService _blogService;
        public HelloWorldController(IHelloWorldService helloWorldService, IBlogService blogService)
        {
            _helloWorldService = helloWorldService;
            _blogService = blogService;
        }

        /// <summary>
        /// ≤‚ ‘Ω”ø⁄
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ResponseResult> HelloWorld()
        {
            return ResponseResult.IsSuccess(await _helloWorldService.HelloWorld());
        }
        /// <summary>
        /// ª∫¥Ê≤‚ ‘
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("cacheSet")]
        public async Task<ResponseResult> CacheSet(string key, string value)
        {
            var rd = new Random();
            await redisHandler.SetAsync(key, value,-1, ToolKits.Cache.RedisType.Common);
            return ResponseResult.IsSuccess(await _blogService.CreateAsync(new PostDto { Author = key+rd.Next(), Title = key+rd.Next(), Html = value+rd.Next() }));
        }

        [HttpPost("setsession")]
        public async Task<IActionResult> SetSession(long tenantId)
        {
            return null;
        }
    }
}