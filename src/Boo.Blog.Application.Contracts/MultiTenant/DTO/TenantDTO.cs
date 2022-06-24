namespace Boo.Blog.MultiTenant.DTO
{
    public class TenantDTO
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 商户code
        /// </summary>
        public string TenantCode { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        public string TenantName { get; set; }
        /// <summary>
        /// 数据库id
        /// </summary>
        public long DatabaseServerId { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
