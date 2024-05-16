using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text;

namespace Jobverse.Models.Resume.Resume
{
    public class PdfService
    {
        private readonly IConverter _converter;

        public PdfService(IConverter converter)
        {
            _converter = converter;
        }
        public byte[] GeneratePdf(string htmlContent)
        {
            var doc = new HtmlToPdfDocument()
            {
                Objects = {
                new ObjectSettings() {
                    HtmlContent = htmlContent,
                }
            }
            };

            return _converter.Convert(doc);
        }
        public string ConvertModelToHtml(ResumeModel model)
        {
            var htmlContent = new StringBuilder();

            // Build the HTML content based on the model
            htmlContent.AppendLine("<!DOCTYPE html>");
            htmlContent.AppendLine("<html lang=\"en\">");
            htmlContent.AppendLine("<head>");
            htmlContent.AppendLine("<style>");
            htmlContent.AppendLine("body { font-family: Arial, sans-serif; }");
            htmlContent.AppendLine("h1, h2, h3 { color: #333; }");
            htmlContent.AppendLine(".experience-item { margin-bottom: 20px; }");
            htmlContent.AppendLine("p { font-size: 17px; }");

            htmlContent.AppendLine("</style>");
            htmlContent.AppendLine("</head>");
            htmlContent.AppendLine("<body>");

            htmlContent.AppendLine($"<h1>{model.FirstName} {model.SecondName}</h1>");
            htmlContent.AppendLine($"<p><b>Email:</b> {model.Email}</p>");
            htmlContent.AppendLine($"<p><b>Phone Number:</b> {model.PhoneNumber}</p>");
            htmlContent.AppendLine($"<p><b>Country:</b> {model.Country}</p>");
            htmlContent.AppendLine($"<p><b>City:</b> {model.City}</p>");
            htmlContent.AppendLine($"<p><b>Summary:</b> {model.Summary}</p>");

            htmlContent.AppendLine("<hr />");
            htmlContent.AppendLine("<h2>Experience</h2>");
            foreach (var experienceItem in model.Experience)
            {
                htmlContent.AppendLine("<div>");
                htmlContent.AppendLine($"<h3>{experienceItem.Title}</h3>");
                htmlContent.AppendLine($"<p>{experienceItem.Company}</p>");
                htmlContent.AppendLine($"<p>{experienceItem.StartDate} - {experienceItem.EndDate}</p>");
                htmlContent.AppendLine($"<p>{experienceItem.Description}</p>");
                htmlContent.AppendLine("</div>");
            }
            htmlContent.AppendLine("<hr />");

            htmlContent.AppendLine("<h2>Education</h2>");
            foreach (var educationItem in model.Education)
            {
                htmlContent.AppendLine("<div>");
                htmlContent.AppendLine($"<h3>{educationItem.LevelOfEducation}</h3>");
                htmlContent.AppendLine($"<p>{educationItem.InstitueName}</p>");
                htmlContent.AppendLine($"<p>{educationItem.City}</p>");
                htmlContent.AppendLine($"<p>{educationItem.From} - {educationItem.To}</p>");
                htmlContent.AppendLine("</div>");
            }

            // Add Skills section
            htmlContent.AppendLine("<hr />");

            htmlContent.AppendLine("<h2>Skills</h2>");
            htmlContent.AppendLine($"<p>{model.Skills}</p>");

            // Add Certificates section
            htmlContent.AppendLine("<hr />");

            htmlContent.AppendLine("<h2>Certificates</h2>");
            htmlContent.AppendLine("<ul>");
            foreach (var certificate in model.Certificates)
            {
                htmlContent.AppendLine($"<li>{certificate}</li>");
            }
            htmlContent.AppendLine("</ul>");

            htmlContent.AppendLine("</body>");
            htmlContent.AppendLine("</html>");

            return htmlContent.ToString();
        }


    }
}
