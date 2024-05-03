using MediatR;
using Resume.Queries;
using Resume.Repository;
using System.Collections.Generic;
namespace Resume.Handlers
{
    public class GetResumeListHandler: IRequestHandler<GetResumeListQuery,List<ResumePdf>>
    {
        private readonly IResumeRepository _repository;
        public GetResumeListHandler(IResumeRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<ResumePdf>> Handle(GetResumeListQuery query, CancellationToken cancellationToken)
        {
            Console.WriteLine("Handler called here");
            return await _repository.getAllResumes();
        }
    }
}
