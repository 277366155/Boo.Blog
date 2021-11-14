namespace Boo.Blog.ToolKits.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty0(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace0(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

    }
}
