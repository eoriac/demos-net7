using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Demo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContextTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContextTypeProvider)
        {
            _fileExtensionContextTypeProvider = fileExtensionContextTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContextTypeProvider));
        }

        [HttpGet("{filePath}")]
        public ActionResult GetFile(string filePath)
        {
            var demoFilePath = "simple-file.pdf";

            if (!System.IO.File.Exists(demoFilePath))
            {
                return NotFound();
            }

            if (!_fileExtensionContextTypeProvider.TryGetContentType(demoFilePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(demoFilePath);
            return File(bytes, contentType, System.IO.Path.GetFileName(filePath));
        }
    }
}
