using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.Authorize.DTO
{
    public class AccessTokenRequestDTO
    {
        public string ClientID = GitHubConfig.ClientID;
        public string ClientSecret = GitHubConfig.ClientSecret;
        public string Code { get; set; }
        public string RedirceUri = GitHubConfig.RedirectUri;
        public string State { get; set; }
    }
}
