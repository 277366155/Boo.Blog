using Boo.Blog.Authorize;
using Boo.Blog.Authorize.DTO;
using Boo.Blog.Response;
using Boo.Blog.ToolKits.Configurations;
using Boo.Blog.ToolKits.Extensions;
using Boo.Blog.ToolKits.JwtUtil;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Boo.Blog.Application.Authorize
{
    //继承ApplicationService，用以供apb框架下的module自动注入
    public class AuthorizeService :ApplicationService, IAuthorizeService
    {
        readonly IHttpClientFactory _httpClient;
        public AuthorizeService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// 生成jwtToken
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<ResponseDataResult<string>> GenerateTokenAsync(string accessToken)
        {
            //GitHubConfig.ApiUser;
            using var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36");
            client.DefaultRequestHeaders.Add("Authorization",$"Token {accessToken}");
            var httpResponse =await  client.GetAsync(GitHubConfig.ApiUser);
            var content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                return ResponseResult.IsFail<string>($"accessToken错误：{content}");
            }
            var userData = content.ToObj<UserResponseDTO>();
            if (userData == null || userData.Id != GitHubConfig.UserId)
            {
                return ResponseResult.IsFail<string>("获取用户信息错误");
            }
            var token = JwtUtil.JwtSecurityToken(
                userData.Name,
                userData.Email,
                AppSettings.Root["Jwt:Domain"],
                AppSettings.Root["Jwt:Expires"].TryToInt(),
                AppSettings.Root["Jwt:SecurityKey"]);

            return ResponseResult.IsSuccess(token);
        }

        /// <summary>
        /// 跟进code获取accessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResponseDataResult<string>> GetAccessTokenAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return await Task.FromResult(ResponseResult.IsFail<string>());
            }

            var request = new AccessTokenRequestDTO();
            var content = new StringContent($"code={code}&client_id={request.ClientID}&redirect_uri={request.RedirceUri}&client_secret={request.ClientSecret}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //C#8.0 using新语法
            using var client = _httpClient.CreateClient();
            var httpResponse = await client.PostAsync(GitHubConfig.ApiAccessToken,content);
            var response = await httpResponse.Content.ReadAsStringAsync();
            if (response.StartsWith("access_token"))
                return ResponseResult.IsSuccess(response.Split("=")[1].Split("&").First());
            else
                return ResponseResult.IsFail<string>("code错误："+response);
        }

        /// <summary>
        ///获取登录地址
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDataResult<string>> GetLoginAddressAsync()
        {
            var request = new AuthorizeRequestDTO();

            var address = string.Concat(new string[] {
                GitHubConfig.ApiAuthorize,
                "?client_id=",request.ClientID,
                "&scope=",request.Scope,
                "&state=",request.State,
               "&redirect_uri=",request.RedirecUri
            });

            var data = ResponseResult.IsSuccess(address);
            return await Task.FromResult(data);
        }
    }
}
