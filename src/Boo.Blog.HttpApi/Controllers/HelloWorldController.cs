using Boo.Blog.Consts;
using Boo.Blog.HelloWorld;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boo.Blog.HttpApi.Controllers
{
    /// <summary>
    /// ����swagger�ĵ�
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
        /// ���Խӿ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> HelloWorld()
        {
            return await  _helloWorldService.HelloWorld();
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("cacheSet")]
        public async Task<bool> CacheSet(string key, string value)
        {
            await redisHandler.SetAsync(key, value,-1, ToolKits.Cache.RedisType.Common);
            return true;
        }
    }
}