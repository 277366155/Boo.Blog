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

        /// <summary>
        /// 根据code获取accessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("GetAccessToken")]
        public async Task<ResponseDataResult<string>> GetAccessTokenAsync(string code)
        {
            return await _authorizeService.GetAccessTokenAsync(code);
        }

        /// <summary>
        ///根据accessToken获取jwtToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        [HttpGet("GenerateToken")]
        public async Task<ResponseDataResult<string>> GenerateTokenAsync(string accessToken)
        {
            return await _authorizeService.GenerateTokenAsync(accessToken);
        }
    }
}
