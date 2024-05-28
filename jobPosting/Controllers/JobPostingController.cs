using jobPosting.Models;
using jobPosting.Repository;
using Microsoft.AspNetCore.Mvc;
using jobPosting.Utils;

namespace jobPosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingController : ControllerBase
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public string AuthToken;

        public JobPostingController(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobPosting>>> Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var jobPosts = await this._jobPostingRepository.GetAllJobPosts();
            return Ok(jobPosts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobPosting>> Get(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var jobPost = await this._jobPostingRepository.GetJobPostById(id);
            return Ok(jobPost);
        }

        [HttpGet("{company}/{_}")]
        public async Task<ActionResult<IEnumerable<JobPosting>>> GetByCompanyName(string company,int _)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var jobPosts = await this._jobPostingRepository.GetJobPostsByCompany(company);
            return Ok(jobPosts);
        }

        [HttpPost]
        public async Task<ActionResult<JobPosting>> Post([FromBody] JobPosting jobPosting)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var createdJobPosting = await this._jobPostingRepository.AddJobPosts(jobPosting);
            return CreatedAtAction(nameof(Get), new { id = createdJobPosting.Id }, createdJobPosting);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] JobPosting jobPosting)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                Console.WriteLine("Token didn't mached");
                return Unauthorized(new { message = "Invalid token" });
            }

            if (id != jobPosting.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var existingJobPosting = await _jobPostingRepository.GetJobPostById(id);
            if (existingJobPosting == null)
            {
                return NotFound(new { message = "Job posting not found" });
            }

            Console.WriteLine("Job Updated");
            await _jobPostingRepository.UpdateJobPost(jobPosting);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var deleted = await _jobPostingRepository.DeleteJobPost(id);
            if (!deleted)
            {
                return NotFound(new { message = "Job posting not found" });
            }
            return NoContent();
        }
    }
}
