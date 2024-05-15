using Microsoft.AspNetCore.Mvc;
using Jobverse.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Authentication.Models;
using static MassTransit.ValidationResultExtensions;
using System.Net.Http;
using Microsoft.AspNetCore.Connections;
using jobPosting.Models;
using ApplyForJob.Models;
using SharedContent.Messages;

namespace Jobverse.Controllers
{
    public class EmployerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public EmployerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult SignupEmployer()
        {
            return View();
        }
        public IActionResult LoginEmployer()
        {
            return View();
        }
        public IActionResult LogoutEmployer()
        {
            ViewBag.ShowSignup = true;
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Company");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult SigninSuccess(Jobverse.Models.Employer employer)
        {
            ViewBag.ShowSignup = true;
            Response.Cookies.Delete("Username");
            Response.Cookies.Append("Company", employer.Company);
            return RedirectToAction("JobsPosted", "Employer");
        }

        public async Task<IActionResult> JobsPosted()
        {
            try
            {
                string company = Request.Cookies["Company"];
                string endpoint = $"https://localhost:7199/api/JobPosting/{company}/{1}";

                var response = await _httpClient.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jobPostings = JsonConvert.DeserializeObject<List<JobPosting>>(content);
                    jobPostings.Reverse();
                    return View(jobPostings);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> JobApplications(int jobId)
        {
            string endpoint = $"https://localhost:7025/api/JobApplication/{jobId}/c?_=c";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jobApplications = JsonConvert.DeserializeObject<List<JobApplication>>(content);
                if(jobApplications.Count > 0)
                {
                    jobApplications.Reverse();
                    return View(jobApplications);
                }
                return View(null);
            }
            else
            {
                return View("Error");
            }
        }
        public async Task<IActionResult> DownloadResume(int jobID,int ResumeId)
        {
            //Console.WriteLine(ResumeId);
            try
            {
                string resumeEndpoint = $"https://localhost:7142/api/resume/{ResumeId}?resumeId={ResumeId}";
                var resumeResponse = await _httpClient.GetAsync(resumeEndpoint);
                if (resumeResponse.IsSuccessStatusCode)
                {
                        var resumeContent = await resumeResponse.Content.ReadAsStringAsync();
                        var resume = JsonConvert.DeserializeObject<ResumePdf>(resumeContent);
                        var resumeBytes = resume.Pdf;
                        string contentType = "application/pdf";
                        return File(resumeBytes, contentType, $"{resume.userEmail}.pdf");
                }
                else
                {
                    return View("Error"); 
                }
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
        }
    }
}
