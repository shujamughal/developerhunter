using MediatR;

namespace Resume.Queries
{
    public class GetResumebyIdQuery: IRequest<ResumePdf>
    {
        public int id {  get; set; }
    }
}
