using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Boo.Blog.CAP
{
    public class BooBlogCAPModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var sp = context.Services.BuildServiceProvider();
            var capOpt = sp.GetService<CAPOption>();
            context.Services.AddCap(a =>
            {
                a.UseMySql(capOpt.MySqlConnection);
                a.UseRabbitMQ(opt =>
                {
                    opt.HostName = capOpt.RabbitMQOpt.HostName;
                    opt.Port = capOpt.RabbitMQOpt.Port;
                    opt.UserName = capOpt.RabbitMQOpt.UserName;
                    opt.Password = capOpt.RabbitMQOpt.Password;
                    opt.VirtualHost = capOpt.RabbitMQOpt.VirtualHost;
                });
                a.Version = "";
                a.DefaultGroupName = capOpt.RabbitMQOpt.DefaultGroupName;
                //a.UseStorageLock = true;
            });
        }
    }
}
