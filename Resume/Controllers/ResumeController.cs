using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this line for ILogger
using Resume.Repository;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly ILogger<ResumeController> _logger; // Add this line

        public ResumeController(IResumeRepository resumeRepository, ILogger<ResumeController> logger)
        {
            _resumeRepository = resumeRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ResumePdf>> Get()
        {
            _logger.LogInformation("Getting all resumes.");
            var resumesList = await _resumeRepository.getAllResumes();
            return resumesList;
        }

        [HttpGet("{id}")]
        public async Task<ResumePdf?> Get(int id)
        {
            _logger.LogInformation($"Getting resume with ID: {id}");
            return await _resumeRepository.getResumebyid(id);
        }

        [HttpPost]
        public async Task<ActionResult<ResumePdf>> Post(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Received a request with an empty file.");
                return BadRequest("File is empty");
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var resume = new ResumePdf { Pdf = ms.ToArray() };

                    var addedResume = await _resumeRepository.AddResume(resume);

                    _logger.LogInformation($"Resume added successfully ");

                    return Ok(addedResume);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing the resume: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        // Other actions...

    }
}
