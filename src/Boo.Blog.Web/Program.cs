using System.Threading.Tasks;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;

namespace Boo.Blog.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(AppSettings.Root)
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                //.WriteTo.Console(new RenderedCompactJsonFormatter())

                //要使用RenderedCompactJsonFormatter类型的格式才能正确替换模板参数
                .WriteTo.File(formatter: new RenderedCompactJsonFormatter(), "logs\\log_.txt", rollingInterval: RollingInterval.Day)        
                .CreateLogger();

            await Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<Startup>().UseSerilog(dispose: true);
            }).UseAutofac().Build().RunAsync();
        }
    }
}
