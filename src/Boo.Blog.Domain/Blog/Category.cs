using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace Boo.Blog.Domain.Blog
{
    public class Category : Entity<long>
    {
        public string CategoryName { get; set; }
        public string DisplayName { get; set; }
    }
}
