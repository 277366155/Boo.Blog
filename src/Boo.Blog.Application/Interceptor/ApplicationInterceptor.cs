using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.DynamicProxy;

namespace Boo.Blog.Application
{
    /// <summary>
    /// application层的拦截器
    /// </summary>
    public  class ApplicationInterceptor:AbpInterceptor
    {
        readonly Stopwatch sw = new Stopwatch();
        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var methodPath = invocation.Method.DeclaringType.Name + "." + invocation.Method.Name;
            sw.Restart();
            await invocation.ProceedAsync();
            sw.Stop();
            Log.Warning($"[{methodPath}]方法执行时长为[ {sw.Elapsed.TotalMilliseconds} ms]。。。");
        }
    }
}
