using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PoseidonPool.Application.Abstractions.Storage;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormCollection form)
        {
            // expects multipart/form-data with files and optional 'path' field
            var files = form.Files;
            var path = form.ContainsKey("path") ? form["path"].ToString() : string.Empty;
            if (files == null || files.Count == 0) return BadRequest("No files provided.");

            var result = await _storageService.UploadAsync(path, files);
            return Ok(result);
        }
    }
}
