using MediatR;

namespace Resume.Queries
{
    public class GetResumebyIdQuery: IRequest<ResumePdf>
    {
        public int ResumeId {  get; set; }
    }
}
