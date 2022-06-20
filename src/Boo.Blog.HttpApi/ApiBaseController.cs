using Boo.Blog.ToolKits.Cache;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Boo.Blog
{
    [ApiController]
    [Route("[controller]")]
    public class ApiBaseController : AbpController
    {
        public IRedisHandler redisHandler => LazyServiceProvider.LazyGetRequiredService<IRedisHandler>();
      
    }
}
