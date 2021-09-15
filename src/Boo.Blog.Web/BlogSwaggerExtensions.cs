using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Boo.Blog.Web
{
    public  static class BlogSwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
          return   services.AddSwaggerGen(opt=> {
                opt.SwaggerDoc("v1",new OpenApiInfo() { 
                    Version = "1.0.0",
                    Title="my interface",
                    Description="接口文档描述"
                });
              opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Boo.Blog.Domain.xml"));
              opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Boo.Blog.Application.Contracts.xml"));
          });
        }

        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder builder)
        {
            return builder.UseSwaggerUI(config=> {
                config.SwaggerEndpoint("/swagger/v1/swagger.json","接口文档");
            });
            
        }
    }
}
