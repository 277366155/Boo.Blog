using Boo.Blog.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Consts;
using Boo.Blog.Domain.Blog;
using Boo.Blog.Paged;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Boo.Blog.Controllers
{
    //[Authorize]
    [ApiExplorerSettings(GroupName = SwaggerGrouping.GroupNameV2)]
    public class BlogController : ApiBaseController
    {
        readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService; //LazyServiceProvider.LazyGetRequiredService<IBlogService>();
        }
      
        /// <summary>
        ///获取博文总数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBlogCount")]
        public async Task<IActionResult> GetBlogCount()
        {
            var data = await _blogService.GetPostsCountAsync();
            return Json(data);
        }

        /// <summary>
        ///创建博文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> InsertBlogAsync(PostDto input)
        {
            var data = await _blogService.CreateAsync(input);
            return Json(data);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpPost("getList")]
        public async Task<IActionResult> GetListAsync(PageParam<Post> page)
        {
            var data = await _blogService.GetListAsync(page);
            return Json(data);
        }

        /// <summary>
        ///获取博客详情
        /// </summary>
        /// <param name="id">博客id</param>
        /// <returns></returns>
        [HttpGet("GetBlogInfo/{id}")]
        public async Task<IActionResult> Test(long id)
        {
            var data = await _blogService.GetPostFullInfoAsync(id);
            return Json(data);
        }

        /// <summary>
        /// 删除post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("deletePost")]
        public async Task Delete(int id)
        { 
           await _blogService.DeletePostAsync(id);
        }

        /// <summary>
        /// uow测试
        /// </summary>
        /// <returns></returns>
        [HttpPost("testuow")]
        public async Task TestUow()
        {
            await _blogService.TestUowAsync();
        }
    }
}
