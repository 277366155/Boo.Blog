﻿using Boo.Blog.Middleware;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Boo.Blog
{
    [DependsOn(
        typeof(AbpIdentityHttpApiModule),
        typeof(BlogApplicationModule)
        )]
    public class BlogHttpApiModule : AbpModule
    {
    }
}
