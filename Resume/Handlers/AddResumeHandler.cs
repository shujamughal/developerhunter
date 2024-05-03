using MediatR;
using Resume.Commands;
using Resume.Repository;

namespace Resume.Handlers
{
    public class AddResumeHandler : IRequestHandler<AddResumeCommand,ResumePdf>
    {
        private readonly IResumeRepository resumeRepository;
        public AddResumeHandler(IResumeRepository resumeRepository)
        {
            this.resumeRepository = resumeRepository;
        } 
        public async Task<ResumePdf> Handle(AddResumeCommand command, CancellationToken cancellationToken)
        {
            var resumePdf = new ResumePdf()
            {
                userEmail = command.userEmail,
                Pdf = command.pdf
            };
            return await resumeRepository.AddResume(resumePdf); 
        }
    }
}
