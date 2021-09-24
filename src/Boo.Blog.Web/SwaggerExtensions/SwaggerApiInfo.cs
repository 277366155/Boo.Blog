using Microsoft.OpenApi.Models;

namespace Boo.Blog.Web
{
    internal class SwaggerApiInfo
    {
        public string UrlPrefix { get; set;  }
        public string Name { get; set; }
        public OpenApiInfo OpenApiInfo { get; set; }
    }
}
