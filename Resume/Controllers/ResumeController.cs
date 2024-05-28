using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Resume.RabbitMQ;
using MediatR;
using Resume.Queries;
using Resume.Commands;
using SharedContent.Messages;
using Resume.Utils;

namespace Resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeIdProducer _resumeIdProducer;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator _mediator;
        public ResumeController(IResumeIdProducer resumeIdProducer,IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _resumeIdProducer = resumeIdProducer;
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async  Task<List<ResumePdf>> Get()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return null;
            }

            var resumesList = await _mediator.Send(new GetResumeListQuery());
            return resumesList;
        }

        // GET api/<ResumeController>/5
        [HttpGet("{id}")]
        public async Task<ResumePdf?> Get(int resumeId)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return null;
            }

            return await _mediator.Send(new GetResumebyIdQuery() { ResumeId = resumeId});
        }

        // POST api/<ResumeController>
        [HttpPost]
        public async Task<ActionResult<ResumePdf>> Post([FromForm] IFormFile file, [FromForm] string userEmail)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                Console.WriteLine("Token not matched");
                return null;
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("User email is required");
            }
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var resume = new ResumePdf { userEmail = userEmail, Pdf = ms.ToArray() };
                var addedResume = await _mediator.Send(new AddResumeCommand(resume.userEmail,resume.Pdf));
                var resumeIdAsync = new ResumeId();
                resumeIdAsync.UserResumeId = addedResume.ResumeId;
                await _publishEndpoint.Publish<ResumeId>(resumeIdAsync);
                return Ok(addedResume);
            }
        }

		//GET api/<ResumeController>
		[HttpGet("resumes/{email}")]
		public async Task<ActionResult<List<ResumePdf>>> GetResumes(string email)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return null;
            }

            List<ResumePdf> resumes = await _mediator.Send(new GetResumesbyEmailQuery() { userEmail= email});
			if (resumes == null || resumes.Count == 0)
			{
				return NotFound(); 
			}

			return resumes;
		}
		// PUT api/<ResumeController>/5
		[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return;
            }

        }

        // DELETE api/<ResumeController>/5
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (token != CompanyTokenManager.CompanyTokenString && token != TokenManager.TokenString)
            {
                return 0;
            }

            return await _mediator.Send(new DeleteResumeCommand() { Id = id });
        }
    }
}
