using Microsoft.EntityFrameworkCore;
using TangPoem.Core.Poems;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace TangPoem.EF
{
    //[ConnectionStringName("MySql")]
    public class PoemDbContext : AbpDbContext<PoemDbContext>
    {
        public DbSet<Poet> Poets { get; set; }
        public DbSet<Poem> Poems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryPoem> CategoriesPoem { get; set; }

        public PoemDbContext(DbContextOptions<PoemDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Poem>(a =>
            {
                a.HasOne(s => s.Author)
                .WithMany(s => s.Poems)
                .HasForeignKey(s => s.PoetId);
            });
            builder.Entity<CategoryPoem>(a=> {
                a.HasKey(s => new { s.CategoryId, s.PoemId });
                a.HasOne(s => s.Poem)
                .WithMany(s => s.PoemCategories)
                .HasForeignKey(s => s.PoemId);
                a.HasOne(s=>s.Category)
                .WithMany(s=>s.CategoryPoems)
                .HasForeignKey(s=>s.CategoryId);
            });
            base.OnModelCreating(builder);
        }
    }
}
