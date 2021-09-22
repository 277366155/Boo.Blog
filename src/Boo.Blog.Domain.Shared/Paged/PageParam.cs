namespace Boo.Blog.Paged
{
    public class PageParam:PageBase
    {
        public string Sort { get; set; }

        //todo：后续完善。基类中无法加入filter过滤条件
        //public string Filter { get; set; }
    }
}
