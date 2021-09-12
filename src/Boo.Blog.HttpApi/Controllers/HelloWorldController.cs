using Boo.Blog.Application.HelloWorld;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Boo.Blog.HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloWorldController : AbpController
    {
        readonly IHelloWorldService _helloWorldService;
        public HelloWorldController(IHelloWorldService helloWorldService)
        {
            _helloWorldService = helloWorldService;
        }

        [HttpGet]
        public string HelloWorld()
        {
            return _helloWorldService.HelloWorld();
        }
    }
}