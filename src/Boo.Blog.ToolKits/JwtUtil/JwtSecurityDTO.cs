namespace Boo.Blog.ToolKits.JwtUtil
{
    public class JwtSecurityDTO
    {
        public string tenantCode { get; set; }
        public string tenantName { get; set; }
        public int exp { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
    }
}
