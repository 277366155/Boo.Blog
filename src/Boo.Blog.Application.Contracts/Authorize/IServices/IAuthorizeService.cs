using Boo.Blog.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Boo.Blog.Authorize
{
    public interface IAuthorizeService//:IServiceBase<>
    {
        Task<string> GetLoginAddressAsync();

        Task<string> GetAccessTokenAsync(string code);

        Task<string> GenerateTokenAsync(string accessToken);
    }
}
