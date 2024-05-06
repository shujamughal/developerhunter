using MediatR;
namespace Resume.Queries
{
    public class GetResumesbyEmailQuery:IRequest<List<ResumePdf>>
    {
        public string userEmail { get; set; }
    }
}
