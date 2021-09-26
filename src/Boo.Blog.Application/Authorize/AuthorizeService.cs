using Boo.Blog.Authorize;
using Boo.Blog.Authorize.DTO;
using Boo.Blog.Response;
using System;
using System.Linq;
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

        public Task<ResponseDataResult<string>> GenerateTokenAsync(string accessToken)
        {
            throw new NotImplementedException();
        }

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
