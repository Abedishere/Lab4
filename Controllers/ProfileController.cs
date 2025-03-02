using Microsoft.AspNetCore.Mvc;
using InmindLab3_4part2.Data;
using InmindLab3_4part2.Services;
using Microsoft.AspNetCore.Http;

namespace InmindLab3_4part2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly UniversityContext _context;
        private readonly IBlobStorageService _blobService;

        public ProfileController(UniversityContext context, IBlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        [HttpPost("student/{id}/picture")]
        // Minimal version: no [Authorize], no [FromForm], no [Consumes]
        public IActionResult UploadStudentProfilePicture(int id, IFormFile file)
        {
            return Ok("Minimal upload action. ID=" + id + ", file=" + file?.FileName);
        }
    }
}