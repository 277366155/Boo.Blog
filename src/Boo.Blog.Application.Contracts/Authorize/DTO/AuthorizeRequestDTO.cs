using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog.Authorize.DTO
{
    public class AuthorizeRequestDTO
    {
        public string ClientID = GitHubConfig.ClientID;
        public string RedirecUri = GitHubConfig.RedirectUri;
        public string State { get; set; } = Guid.NewGuid().ToString("N");
        public string Scope { get; set; } = "user,public_repo";
    }
}
