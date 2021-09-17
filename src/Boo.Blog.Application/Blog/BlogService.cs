using Volo.Abp.Application.Services;
using Boo.Blog.Application.Contracts.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using Boo.Blog.Domain.Blog.IRepositories;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Boo.Blog.Application.Blog
{
    public class BlogService : CrudAppService<Post, PostDto, long>, IBlogService
    {
        public BlogService(IRepository<Post,long> repository) : base(repository)
        {
        }

        public async Task<int> BulkInsertAsync(int n)//(IEnumerable<PostDto> postDto)
        {
            return await Task.Run(() =>n);
        }
    }
}
