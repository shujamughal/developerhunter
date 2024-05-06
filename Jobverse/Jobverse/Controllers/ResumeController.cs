using DinkToPdf;
using DinkToPdf.Contracts;
using Jobverse.Models;
using Jobverse.Models.Resume.Resume;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Add this line
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jobverse.Controllers
{
    public class ResumeController : Controller
    {
        private readonly PdfService _pdfService;
        private readonly ILogger<ResumeController> _logger; // Add this line

        public ResumeController(PdfService pdfService, ILogger<ResumeController> logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        public IActionResult buildResume()
        {
            _logger.LogInformation("Rendering buildResume view.");
            return View();
        }

        public IActionResult reviewResume(ResumeModel m)
        {
            _logger.LogInformation("Rendering reviewResume view.");
            return View(m);
        }

        public async Task<IActionResult> GeneratePdf(ResumeModel model)
        {
            _logger.LogInformation("Generating PDF.");

            try
            {
                string htmlContent = _pdfService.ConvertModelToHtml(model);
                var pdfBytes = _pdfService.GeneratePdf(htmlContent);

                using (var pdfStream = new MemoryStream(pdfBytes))
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri("https://localhost:7142/");

                        // Prepare form data
                        string userEmail = "ahmadAli@gmail.com"; 
                        if (string.IsNullOrEmpty(userEmail))
                        {
                            return BadRequest("User email is required");
                        }

                        var formContent = new MultipartFormDataContent();
                        formContent.Add(new StreamContent(pdfStream), "file", "Resume.pdf");
                        formContent.Add(new StringContent(userEmail), "userEmail");

                        Console.WriteLine(formContent);
                        var response = await httpClient.PostAsync("api/resume", formContent);
                        Console.WriteLine(response);
                        if (response.IsSuccessStatusCode)
                        {
                            var resultString = await response.Content.ReadAsStringAsync();

                            var result = JsonConvert.DeserializeObject<ResumePdf>(resultString);

                            _logger.LogInformation("PDF generated and uploaded successfully.");

                            // Return the PDF file
                            return File(pdfBytes, "application/pdf", "Resume.pdf");
                        }
                        else
                        {
                            _logger.LogError($"Error uploading PDF to API. Status code: {response.StatusCode}");
                            return StatusCode((int)response.StatusCode, "Error uploading PDF to API");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError($"Error generating PDF: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}