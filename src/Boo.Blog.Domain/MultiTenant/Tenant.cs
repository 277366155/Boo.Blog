using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.MultiTenant
{
    public class Tenant : Entity, ITenant
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 商户code
        /// </summary>
        [StringLength(64)]
        public string TenantCode { get; set; }
        /// <summary>
        /// 商户名称
        /// </summary>
        [StringLength(64)]
        public string TenantName { get; set; }
        /// <summary>
        /// 数据库id
        /// </summary>
        public long DatabaseServerId { get; set;}
        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
        public virtual DatabaseServer DatabaseServer { get; set; }

        public override object[] GetKeys()
        {
            return new object[1]
            {
                Id
            };
        }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }
}
