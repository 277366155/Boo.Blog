using Boo.Blog.Application.HelloWorld;
using Boo.Blog.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        public bool CacheSet(string key, string value)
        {
            return _helloWorldService.CacheTest(key, value);
        }
    }
}