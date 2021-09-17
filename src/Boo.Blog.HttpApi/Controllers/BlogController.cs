using Boo.Blog.Application.Contracts.Blog;
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
    public class BlogController:AbpController
    {
        IBlogService _blogService;
        public BlogController(IBlogService blogService)
        { 
            _blogService= blogService; 
        }

        /// <summary>
        ///根据id获取博文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBlog(int id)
        {
            var data= await _blogService.BulkInsertAsync(id);
            return Json(data);
        }
    }
}
