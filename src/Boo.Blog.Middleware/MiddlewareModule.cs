using Volo.Abp.Modularity;

namespace Boo.Blog.Middleware
{
    /// <summary>
    /// 如果当前项目中，有声明需自动注入的类，就需要声明当前Module，并再相关项目的Module声明DependOn依赖
    /// </summary>
    public class MiddlewareModule:AbpModule
    {
    }
}
