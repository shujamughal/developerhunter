using MediatR;

namespace Resume.Commands
{
    public class AddResumeCommand : IRequest<ResumePdf>
    {
        public string userEmail { get; set; }
        public byte[] pdf { get; set; }
        public AddResumeCommand(string userEmail, byte[] pdf)
        {
            this.userEmail= userEmail;
            this.pdf = pdf;
        }
    }
}
