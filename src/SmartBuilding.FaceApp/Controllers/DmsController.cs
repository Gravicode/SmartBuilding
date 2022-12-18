using Microsoft.AspNetCore.Mvc;
using SmartBuilding.FaceApp.Data;

namespace SmartBuilding.FaceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DmsController : Controller
    {
        AzureBlobHelper blob;
        public DmsController(AzureBlobHelper blob)
        {
            this.blob = blob;

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetFile(string filename)
        {
            /*
            var res = await blob.DownloadFile(fileName);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(res)
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            */
            var file = await blob.DownloadFile(filename);
            if (file != null)
            {
                //var stream = new MemoryStream(file);
                var mime = MimeTypeHelper.GetMimeType(Path.GetExtension(filename));
                return File(file, mime,filename);
            }
            return NotFound();
        }
        
    }
}
