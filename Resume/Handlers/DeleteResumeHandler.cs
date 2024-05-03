using MediatR;
using Resume.Commands;
using Resume.Repository;

namespace Resume.Handlers
{
    public class DeleteResumeHandler: IRequestHandler<DeleteResumeCommand,int>
    {
        private readonly IResumeRepository _repository;
        public DeleteResumeHandler(IResumeRepository repository)
        {
            _repository = repository;
        }
        public async Task<int> Handle(DeleteResumeCommand command , CancellationToken cancellationToken)
        {
            return await _repository.DeleteResume(command.Id);
        }
    }
}
