using Boo.Blog.Authorize;
using Boo.Blog.Consts;
using Boo.Blog.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boo.Blog.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName= SwaggerGrouping.GroupNameV2)]
    public class AuthController : ApiBaseController
    {
        readonly IAuthorizeService _authorizeService;
        public AuthController(IAuthorizeService authorizeService)
        {
            _authorizeService= authorizeService; 
        }

        /// <summary>
        /// 获取登录地址（GitHub）
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUrl")]
        public async Task<ResponseDataResult<string>> GetLoginAddressAsync()
        {
            return await _authorizeService.GetLoginAddressAsync();
        }

        [HttpGet("GetAccessToken")]
        public async Task<ResponseDataResult<string>> GetAccessTokenAsync(string code)
        {
            return await _authorizeService.GetAccessTokenAsync(code);
        }
    }
}
