using System.Collections.Generic;

namespace Boo.Blog.Paged
{
    public class PageResult<T> : PageBase
    {
        public int PageCount => TotalCount / PageSize + (TotalCount % PageSize > 0 ? 1 : 0);
        public int TotalCount { get; set; }

        public List<T> Items { get; set; }
    }
}
