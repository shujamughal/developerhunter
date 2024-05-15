using MediatR;
using Resume.Queries;
using Resume.Repository;

namespace Resume.Handlers
{
    public class GetResumebyIdHandler : IRequestHandler<GetResumebyIdQuery, ResumePdf>
    {
        private readonly IResumeRepository _resumeRepository;
        public GetResumebyIdHandler(IResumeRepository resumeRepository) 
        {
            _resumeRepository = resumeRepository;
        }
        public async Task<ResumePdf> Handle(GetResumebyIdQuery query, CancellationToken cancellationToken)
        {
            return await _resumeRepository.getResumebyid(query.ResumeId);
        }
    }
}
