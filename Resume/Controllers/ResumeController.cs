using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;
using Resume.RabbitMQ;
using Resume.Repository;
using Resume.Resume;
using MediatR;
using Resume.Queries;
using Resume.Commands;
using Microsoft.AspNetCore.Identity;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeIdProducer _resumeIdProducer;
        //private readonly IResumeRepository _resumeRepository;
        private readonly IMediator _mediator;
        public ResumeController(IResumeIdProducer resumeIdProducer,IMediator mediator)
        {
            //_resumeRepository = resumeRepository;
            _resumeIdProducer = resumeIdProducer;
            _mediator = mediator;
        }

        // GET: api/<ResumeController>
        [HttpGet]
        public async  Task<List<ResumePdf>> Get()
        {
            var resumesList = await _mediator.Send(new GetResumeListQuery());
            return resumesList; 
        }

        // GET api/<ResumeController>/5
        [HttpGet("{id}")]
        public async Task<ResumePdf?> Get(int resumeId)
        {
            return await _mediator.Send(new GetResumebyIdQuery() { id = resumeId});
        }

        // POST api/<ResumeController>
        [HttpPost]
        public async Task<ActionResult<ResumePdf>> Post([FromForm] IFormFile file, [FromForm] string userEmail)
        {
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
                //Console.WriteLine(addedResume.);
                return Ok(addedResume);
            }
        }

		//GET api/<ResumeController>
		[HttpGet("resumes/{email}")]
		public async Task<ActionResult<List<ResumePdf>>> GetResumes(string email)
		{
            Console.WriteLine("In Api");
			List<ResumePdf> resumes = await _mediator.Send(new GetResumesbyEmailQuery() { userEmail= email});
			if (resumes == null || resumes.Count == 0)
			{
                Console.WriteLine("Uuuuuuuuu");
				return NotFound(); 
			}

			return resumes;
		}
		// PUT api/<ResumeController>/5
		[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ResumeController>/5
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _mediator.Send(new DeleteResumeCommand() { Id = id });
        }
    }
}
