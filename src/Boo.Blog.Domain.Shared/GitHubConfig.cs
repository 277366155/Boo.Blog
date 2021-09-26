using Boo.Blog.ToolKits.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Boo.Blog
{
    public class GitHubConfig
    {
        public static string ApiAuthorize = "https://github.com/login/oauth/authorize";
        public static string ApiAccessToken = "https://github.com/login/oauth/access_token";
        public static string ApiUser = "https://api.github.com/user";

        public static int UserId =int.Parse( AppSettings.Root["GitHub:UserId"]);
        public static string ClientID = AppSettings.Root["GitHub:ClientID"];
        public static string ClientSecret = AppSettings.Root["GitHub:Secert"];
        public static string RedirectUri = AppSettings.Root["GitHub:RedirectUri"];
        public static string ApplicationName = AppSettings.Root["GitHub:ApplicationName"];
    }
}
