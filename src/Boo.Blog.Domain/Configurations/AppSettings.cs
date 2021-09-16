using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Boo.Blog.Domain.Configurations
{
    public class AppSettings
    {
        //private static readonly IConfigurationRoot _config;
        private static IConfigurationBuilder _builder;
        private static object lockObj = new object();
        static AppSettings()
        {
            _builder = InitConfigurationBuilder();
        }
        /// <summary>
        /// 默认读取appsettings.json文件
        /// </summary>
        /// <param name="act">可添加其他配置文件</param>
        /// <returns></returns>
        public static IConfigurationBuilder InitConfigurationBuilder(Action<IConfigurationBuilder> act = null)
        {

            if (_builder == null)
            {
                lock (lockObj)
                {
                    if (_builder == null)
                    {
                        var builder = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                        _builder = builder;
                        ChangeToken.OnChange(
                            () => Configuration.GetReloadToken(),
                            () => Configuration = _builder.Build());
                    }
                }
            }
            if (act != null)
            {
                act?.Invoke(_builder);
                configuration = _builder.Build();
            }

            return _builder;
        }

        static IConfigurationRoot configuration;
        static object configLockObj = new object();

        /// <summary>
        /// 读取配置
        /// </summary>
        public static IConfigurationRoot Configuration
        {
            get
            {
                if (configuration == null)
                {
                    lock (configLockObj)
                    {
                        if (configuration == null)
                        {
                            configuration = _builder.Build();
                        }
                    }
                }
                return configuration;
            }
            set
            {
                configuration = value;
            }
        }

        public static string EnableDb => Configuration["ConnectionStrings:EnableDb"];
        /// <summary>
        /// 读取根节点下的appsetting节点
        /// </summary>
        public static IConfigurationSection AppSetting
        {
            get
            {
                return Configuration.GetSection("AppSetting");
            }
        }
    }
}
