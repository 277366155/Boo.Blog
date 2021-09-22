using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Boo.Blog.Paged
{
    public class PageParam<T>:PageBase
    {
        public  Dictionary<string,bool> Sorts { get; set; }

        //todo：后续完善。基类中无法加入filter过滤条件
        //public Expression<Func<T, bool>> Filters { get; set; }
    }
}
