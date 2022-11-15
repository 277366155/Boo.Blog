using DotNetCore.CAP.Internal;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;

namespace Boo.Blog.CAP
{
    public static class CapServiceCollectionExtensions
    {
        public static ServiceConfigurationContext AddAbpCap(this ServiceConfigurationContext context, CAPOption capOpt)
        {

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

            context.Services.AddSingleton<IConsumerServiceSelector, CapConsumerServiceSelector>();
            context.Services.AddSingleton<IDistributedEventBus, CapDistributedEventBus>();
            return context;
        }
    }
}
