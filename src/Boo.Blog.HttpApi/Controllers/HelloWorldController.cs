using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Consts;
using Boo.Blog.Domain.MultiTenant;
using Boo.Blog.HelloWorld;
using Boo.Blog.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boo.Blog.HttpApi.Controllers
{
    /// <summary>
    /// ≤‚ ‘swaggerŒƒµµ
    /// </summary>
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
        public async Task<string> HelloWorld()
        {
            return await  _helloWorldService.HelloWorld();
        }
        /// <summary>
        /// ª∫¥Ê≤‚ ‘
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("cacheSet")]
        public async Task<ResponseDataResult<PostDto>> CacheSet(string key, string value)
        {
            await redisHandler.SetAsync(key, value,-1, ToolKits.Cache.RedisType.Common);
            return await _blogService.CreateAsync(new Blog.DTO.PostDto { Author = key, Title = key, Html = value });

        }
    }
}