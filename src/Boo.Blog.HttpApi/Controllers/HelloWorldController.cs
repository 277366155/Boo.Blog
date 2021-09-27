using Boo.Blog.Application.HelloWorld;
using Boo.Blog.Consts;
using Microsoft.AspNetCore.Mvc;

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
        public string HelloWorld()
        {
            return _helloWorldService.HelloWorld();
        }
    }
}