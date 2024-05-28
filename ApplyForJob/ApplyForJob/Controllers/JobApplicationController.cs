using ApplyForJob.Models;
using ApplyForJob.Repository;
using Microsoft.AspNetCore.Mvc;
using ApplyForJob.Utils;
using System;
using SharedContent.Messages;

namespace ApplyForJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public JobApplicationController(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobApplication>>> Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            var jobApplications = await this._jobApplicationRepository.GetAllJobApplications();
            return Ok(jobApplications);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobApplication>> Get(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            var jobApplication = await this._jobApplicationRepository.GetJobApplicationById(id);
            if (jobApplication == null)
                return NotFound();
            return Ok(jobApplication);
        }

        [HttpGet("{id}/{company}")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetByJobId(int id, string _)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }
            var jobApplications = await this._jobApplicationRepository.GetJobApplicationByJobId(id);
            if (jobApplications == null)
                return NotFound();
            return Ok(jobApplications);
        }

        [HttpGet("MyJob")]
        public async Task<ActionResult<IEnumerable<JobApplication>>> GetJobsByUsername([FromQuery] string username)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var jobPostsByUsername = await _jobApplicationRepository.GetJobApplicationsByUsername(username);
                return Ok(jobPostsByUsername);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving job postings by username: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<JobApplication>> Post([FromBody] JobApplication jobApplication)
        {
            jobApplication.ResumeId = UserResumeId.ResumeId;

            // Extract the token from the request headers
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var createdJobApplication = await _jobApplicationRepository.AddJobApplication(jobApplication);
            return CreatedAtAction(nameof(Get), new { id = createdJobApplication.Id }, createdJobApplication);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] JobApplication jobApplication)
        {
            // Extract the token from the request headers
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            if (id != jobApplication.Id)
                return BadRequest();

            var updatedNotification = await this._jobApplicationRepository.UpdateJobApplication(jobApplication);
            if (updatedNotification == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Extract the token from the request headers
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var jobApplication = await this._jobApplicationRepository.GetJobApplicationById(id);
            if (jobApplication == null)
                return NotFound();

            await this._jobApplicationRepository.DeleteJobApplication(id);

            return NoContent();
        }

        [HttpDelete("{id}/{email}")]
        public async Task<IActionResult> Delete(int id, string email)
        {
            // Extract the token from the request headers
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            var jobApplications = await this._jobApplicationRepository.GetJobApplicationByJobId(id);
            foreach(JobApplication application in jobApplications)
            {
                if(application.UserEmail == email)
                {
                    await this._jobApplicationRepository.DeleteJobApplication(application.Id);
                    return NoContent();
                }
            }
            return NoContent();
        }
    }
}
