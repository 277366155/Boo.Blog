using Boo.Blog.Domain.Blog;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace Boo.Blog.EntityFrameworkCore
{
    //[ReplaceDbContext(typeof(IIdentityDbContext))]
    [ConnectionStringName("MySql")]
    public class BlogDbContext : AbpDbContext<BlogDbContext>//,IIdentityDbContext        
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */

        #region Entities from the modules

        ////Identity
        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
        //public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        //public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        //public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        //public DbSet<IdentityLinkUser> LinkUsers { get; set; }

        #endregion

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<FriendLink> FriendLinks { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Configure();

            //builder.ConfigureIdentity();

        }
    }
}
