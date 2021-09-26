using Boo.Blog.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Boo.Blog.Authorize
{
    public interface IAuthorizeService//:IServiceBase<>
    {
        Task<ResponseDataResult<string>> GetLoginAddressAsync();

        Task<ResponseDataResult<string>> GetAccessTokenAsync(string code);

        Task<ResponseDataResult<string>> GenerateTokenAsync(string accessToken);
    }
}
