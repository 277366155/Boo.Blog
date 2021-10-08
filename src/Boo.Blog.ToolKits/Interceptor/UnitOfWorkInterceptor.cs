using Serilog;
using System;
using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;

namespace Boo.Blog.ToolKits.Interceptor
{
    public class UnitOfWorkInterceptor : AbpInterceptor
    {
        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            Log.Warning("执行前。。。");
            await invocation.ProceedAsync();
            Log.Warning("执行后。。。");
        }
    }
}
