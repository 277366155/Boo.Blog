using Volo.Abp.Domain.Entities;

namespace TangPoem.Core.Poems
{
    public class Poet : Entity<long>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
