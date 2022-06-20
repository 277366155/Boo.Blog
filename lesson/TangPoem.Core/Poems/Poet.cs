using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace TangPoem.Core.Poems
{
    public class Poet : Entity<long>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Poem> Poems { get; set; }
    }
}
