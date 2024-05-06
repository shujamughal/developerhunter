using MediatR;
using Resume.Queries;
using Resume.Repository;

namespace Resume.Handlers
{
    public class GetResumesbyEmailHandler: IRequestHandler<GetResumesbyEmailQuery,List<ResumePdf>>
    {
        private readonly IResumeRepository resumeRepository;
        public GetResumesbyEmailHandler(IResumeRepository resumeRepository)
        {
            this.resumeRepository = resumeRepository;
        }
        public async Task<List<ResumePdf>> Handle (GetResumesbyEmailQuery query , CancellationToken cancellationToken)
        {
            return await resumeRepository.getResumesbyEmail(query.userEmail);
        }
    }
}
