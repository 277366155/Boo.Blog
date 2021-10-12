using Abp.Modules;
using System.Reflection;

namespace ConsolePro
{
    public class ConsoleClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
