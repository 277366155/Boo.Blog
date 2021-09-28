using Boo.Blog.Consts;
using Boo.Blog.ToolKits.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;

namespace Boo.Blog.Web
{
    public static class BlogSwaggerExtensions
    {
        static readonly string apiVersion = AppSettings.AppSetting["ApiVersion"] ?? "v1";
        static readonly string description = AppSettings.AppSetting["Description"] ?? "";

        /// <summary>
        /// api接口组
        /// </summary>
        static readonly List<SwaggerApiInfo> ApiInfos = new List<SwaggerApiInfo>
        {
        new SwaggerApiInfo{
            Name="前台接口",
            UrlPrefix=SwaggerGrouping.GroupNameV1,
            OpenApiInfo= new OpenApiInfo
            {
                Version=apiVersion,
                 Title="前台接口文档",
                  Description=description
            }
        },
        new SwaggerApiInfo{
            Name="JWT授权接口",
            UrlPrefix=SwaggerGrouping.GroupNameV2,
            OpenApiInfo= new OpenApiInfo
            {
                Version=apiVersion,
                 Title="JWT授权接口文档",
                  Description=description
            }
        }
        };
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(opt =>
            {
                #region 授权验证
                var security = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT授权，输入格式为【Bearer {token}】"
                };
                opt.AddSecurityDefinition("oauth2", security);
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { security, new List<string>() }
                });
                opt.OperationFilter<AddResponseHeadersFilter>();
                opt.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                opt.OperationFilter<SecurityRequirementsOperationFilter>();
                #endregion 授权验证

                #region swagger文档分组
                ApiInfos.ForEach(a =>
                {
                    opt.SwaggerDoc(a.UrlPrefix, a.OpenApiInfo);
                });
                #endregion swagger文档分组

                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Boo.Blog.Domain.xml"));
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Boo.Blog.Application.Contracts.xml"));
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Boo.Blog.HttpApi.xml"));
            });
        }

        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder builder)
        {
            return builder.UseSwaggerUI(config =>
            {
                ApiInfos.ForEach(a =>
                {
                    config.SwaggerEndpoint($"/swagger/{a.UrlPrefix}/swagger.json", a.Name);
                });
                // 模型的默认扩展深度，设置为 -1 完全隐藏模型
                config.DefaultModelsExpandDepth(-1);

                // API文档仅展开标记
                config.DocExpansion(DocExpansion.List);

                // API前缀设置为空
                config.RoutePrefix = string.Empty;

                // API页面Title
                config.DocumentTitle = "😍接口文档";
            });

        }
    }
}
