using Volo.Abp.Domain.Entities;

namespace TangPoem.Core.Poems
{
    public  class CategoryPoem:Entity<long>
    {
        public long CategoryId { get; set; }
        public long PoemId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Poem Poem { get; set; }
    }
}
