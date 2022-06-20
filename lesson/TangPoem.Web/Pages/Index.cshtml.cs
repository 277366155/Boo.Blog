using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TangPoem.Application.Poems;

namespace TangPoem.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        IPoemApplicationService poemApplicationService;
        public List<CategoryDto> Categories;
        public IndexModel(ILogger<IndexModel> logger,IPoemApplicationService poemApplicationService)
        {
            _logger = logger;
            this.poemApplicationService = poemApplicationService;
        }

        public void OnGet()
        {
            Categories=poemApplicationService.GetAllCategories();
        }
    }
}
