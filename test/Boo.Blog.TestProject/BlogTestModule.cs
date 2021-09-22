using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Modularity;

namespace Boo.Blog.TestProject
{
    [DependsOn(typeof(AbpAutofacModule),
        typeof(BlogApplicationModule),
        typeof(BlogDomainModule))]
    public class BlogTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.OnRegistred(RegisterIfNeeded);
        }

        public static void RegisterIfNeeded(IOnServiceRegistredContext context)
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuditingInterceptor>();
            }
        }
        private static bool ShouldIntercept(Type type)
        {
            if (DynamicProxyIgnoreTypes.Contains(type))
            {
                return false;
            }
            //if (ShouldAuditTypeByDefaultOrNull(type))
            //{ 

            //}
            if (type.GetMethods().Any(a => a.IsDefined(typeof(AuditedAttribute), true)))
            {
                return true;
            }

            return true;
        }
    }

}
