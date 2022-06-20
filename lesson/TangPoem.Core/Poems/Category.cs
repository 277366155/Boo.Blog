using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace TangPoem.Core.Poems
{
    public class Category:Entity<long>
    {
        public string Name { get; set; }
        public virtual ICollection<CategoryPoem> CategoryPoems { get; set; }
    }
}
