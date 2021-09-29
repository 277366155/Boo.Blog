using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.ExceptionHandling;

namespace Boo.Blog.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<BlogWebModule>();
            //services.Configure<AbpExceptionHandlingOptions>(opt =>
            //{
            //    opt.SendExceptionsDetailsToClients = true;
            //});
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
