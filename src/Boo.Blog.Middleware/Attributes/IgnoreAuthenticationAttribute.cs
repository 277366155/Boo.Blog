using System;

namespace Boo.Blog.Middleware.Attributes
{
    /// <summary>
    /// 用于标记忽略授权验证的Controller或Action
    /// </summary>
    public class IgnoreAuthenticationAttribute:Attribute
    {
    }
}
