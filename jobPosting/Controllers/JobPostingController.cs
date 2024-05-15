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
            var jobPosts = await this._jobPostingRepository.GetAllJobPosts();
            return Ok(jobPosts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobPosting>> Get(int id)
        {
            var jobPost = await this._jobPostingRepository.GetJobPostById(id);
            Console.WriteLine("Specific Job, public async Task<ActionResult<JobPosting>> Get(int id)");
            return Ok(jobPost);
        }

        [HttpGet("{company}/{_}")]
        public async Task<ActionResult<IEnumerable<JobPosting>>> GetByCompanyName(string company,int _)
        {
            var jobPosts = await this._jobPostingRepository.GetJobPostsByCompany(company);
            return Ok(jobPosts);
        }

        [HttpPost]
        public async Task<ActionResult<JobPosting>> Post([FromBody] JobPosting jobPosting)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var createdJobPosting = await this._jobPostingRepository.AddJobPosts(jobPosting);
            return CreatedAtAction(nameof(Get), new { id = createdJobPosting.Id }, createdJobPosting);
            //if (token == TokenManager.TokenString)
            //{
            //    Console.WriteLine("Token matched to post job");
            //    var createdJobPosting = await this._jobPostingRepository.AddJobPosts(jobPosting);
            //    return CreatedAtAction(nameof(Get), new { id = createdJobPosting.Id }, createdJobPosting);
            //}

            //return Unauthorized(new { message = "Invalid token" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] JobPosting jobPosting)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            //if (token != TokenManager.TokenString)
            //{
            //    return Unauthorized(new { message = "Invalid token" });
            //}

            if (id != jobPosting.Id)
            {
                return BadRequest(new { message = "ID mismatch" });
            }

            var existingJobPosting = await _jobPostingRepository.GetJobPostById(id);
            if (existingJobPosting == null)
            {
                return NotFound(new { message = "Job posting not found" });
            }

            await _jobPostingRepository.UpdateJobPost(jobPosting);
            Console.WriteLine(".....");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Console.WriteLine("id: ", id);
            var deleted = await _jobPostingRepository.DeleteJobPost(id);
            if (!deleted)
            {
                return NotFound(new { message = "Job posting not found" });
            }
            return NoContent();
        }
    }
}
