using Boo.Blog.Domain.Blog;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Boo.Blog.EntityFrameworkCore
{
    public static class BlogDbContextModelCreatingExtensions
    {
        public static void Configure(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Category>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + b.GetType().Name);
            });
            builder.Entity<FriendLink>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + b.GetType().Name);
            });
            builder.Entity<Post>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + b.GetType().Name);
            });
            builder.Entity<PostTag>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + b.GetType().Name);
            });
            builder.Entity<Tag>(b =>
            {
                b.ToTable(BlogConsts.DbTablePrefix + b.GetType().Name);
            });
        }
    }
}
