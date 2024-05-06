using MediatR;
namespace Resume.Queries
{
    public class GetResumeListQuery : IRequest<List<ResumePdf>>
    {
    }
}
