using AliOSS.WebApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AliOSS.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        AliCloudOSSService _service;
        public DefaultController(AliCloudOSSService service)
        {
            _service = service;
        }

        [HttpPost("UploadFile")]
        public  IActionResult UploadFile(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var result =_service.SimpleUploads(stream,file.FileName);
            return new ContentResult() { Content = result };
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFileAsync(string fileName)
        {
            var result =await _service.SimpleDownloadsAsync(fileName);
            if (result == null)
            {
                return new EmptyResult();
            }
            else
            {
                return new FileStreamResult(result, "application/octet-stream") { FileDownloadName = fileName };
            }
        }

        [HttpPost("DeleteFile")]
        public bool DeleteFile(string fileName)
        {
            var result =  _service.DeleteObject(fileName);
            return  result;
        }

        [HttpPost("CheckExist")]
        public  bool CheckExist(string fileName)
        { 
            var result = _service.DoesObjectExist(fileName);
            return result;
        }
    }
}
