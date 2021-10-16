using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace TangPoem.Core.Poems
{
    public class Poem : Entity<long>
    {
        public string Title{ get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 点评
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 诗人id
        /// </summary>
        public long PoetId { get; set; }

        public virtual Poet Author { get; set; }
        public virtual ICollection<CategoryPoem> PoemCategories { get; set; }
    }
}
