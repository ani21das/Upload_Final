//using autoUploadWebApi.Data;
//using autoUploadWebApi.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace autoUploadWebApi.Controllers
//{

//        [Route("api/[controller]")]
//        [ApiController]
//        public class UploadController : ControllerBase
//        {
//            private readonly ApplicationDbContext _context;

//            public UploadController(ApplicationDbContext context)
//            {
//                _context = context;
//            }

//            [HttpPost("upload")]
//            public async Task<IActionResult> UploadFile(IFormFile file)
//            {
//                if (file == null || file.Length == 0)
//                {
//                    return BadRequest("No file uploaded.");
//                }

//                var filePath = Path.Combine("UploadedFiles", file.FileName);

//                Directory.CreateDirectory("UploadedFiles");

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await file.CopyToAsync(stream);
//                }

//                var fileRecord = new FileRecord
//                {
//                    FileName = file.FileName,
//                    FilePath = filePath,
//                    UploadedAt = DateTime.UtcNow
//                };

//                _context.FileRecords.Add(fileRecord);
//                await _context.SaveChangesAsync();

//                return Ok(new { filePath });
//            }
//        }
//    }




using autoUploadWebApi.Data;
using autoUploadWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace autoUploadWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UploadController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string systemId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var folderPath = Path.Combine("UploadedFiles", systemId);
            var filePath = Path.Combine(folderPath, file.FileName);

            Directory.CreateDirectory(folderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileRecord = new FileRecord
            {
                FileName = file.FileName,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            _context.FileRecords.Add(fileRecord);
            await _context.SaveChangesAsync();

            return Ok(new { filePath });
        }
    }
}
