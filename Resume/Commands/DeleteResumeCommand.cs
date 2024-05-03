using MediatR;
namespace Resume.Commands
{
    public class DeleteResumeCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
