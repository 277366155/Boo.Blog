using Boo.Blog.Application.Contracts.Blog;
using Boo.Blog.Blog.DTO;
using Boo.Blog.Domain.Blog;
using Boo.Blog.Paged;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Boo.Blog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : AbpController
    {
        readonly IBlogService _blogService;
        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        /// <summary>
        ///根据id获取博文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
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
    }
}
