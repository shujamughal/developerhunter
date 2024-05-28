using ApplyForJob.Models;
using jobPosting.Models;
using Jobverse.Models;
using Jobverse.Services;
using Jobverse.Utils;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedContent.Messages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Jobverse.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITokenEncryptionService _encryptionService;

        public JobApplicationController(IHttpClientFactory httpClientFactory, ITokenEncryptionService encryptionService, IPublishEndpoint publishEndpoint)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7025/");
            _encryptionService = encryptionService;
            _publishEndpoint = publishEndpoint;
        }
        public IActionResult JobApplication(int jobId)
        {
            // Check if the "username" cookie exists
            if (Request.Cookies["Username"] != null)
            {
                ViewBag.JobId = jobId;
                return View();
            }
            else
            {
                return RedirectToAction("SignupEmployee", "Employee");
            }
        }
        private async Task<List<ResumePdf>> getResumesEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7142/api/resume/resumes/{email}");
            List<ResumePdf> resumes = new List<ResumePdf>();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                resumes = JsonConvert.DeserializeObject<List<ResumePdf>>(content);
                return resumes;
            }
            else
            {
                return resumes;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveApplication(int JobId, string Applier, string UserEmail, string PhoneNumber, IFormFile resume, int Experience, int resumeId = 0)
        {
            try
            {
                if (resume != null)
                {
                    var addedResumeResponse = await AddResumeAsync(UserEmail, resume, "https://localhost:7142/");
                    if (!addedResumeResponse.IsSuccessStatusCode)
                    {
                        return Json(new { success = false, message = "Error adding resume" });
                    }
                    var addedResume = JsonConvert.DeserializeObject<ResumePdf>(await addedResumeResponse.Content.ReadAsStringAsync());
                    Console.WriteLine("::");
                    Console.WriteLine(addedResume.ResumeId);
                }

                var saveApplicationResponse = await SaveJobApplicationAsync(JobId, Applier, UserEmail, PhoneNumber, resumeId, Experience, "https://localhost:7025/");

                if (!saveApplicationResponse.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Error saving job application" });
                }

				return RedirectToAction("Index", "Home");
			}
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyJob()
        {
            if (Request.Cookies.ContainsKey("Username"))
            {
                try
                {
                    string username = Request.Cookies["Username"];

                    string encryptedToken = _encryptionService.EncryptToken(TokenManager.TokenString);

                    string apiEndpoint = $"https://localhost:7025/api/JobApplication/MyJob?username={username}";

                    // Sending token in the headers
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", encryptedToken);

                    var response = await _httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var jobApplications = JsonConvert.DeserializeObject<List<JobApplication>>(content);
                        int numOfJobApplications = jobApplications != null? jobApplications.Count : 0;
                        List<JobPosting> jobs = new List<JobPosting>();
                        for (int i = 0; i < numOfJobApplications; ++i)
                        {
                            string endpoint = $"https://localhost:7199/api/JobPosting/{jobApplications[i].JobId}";
                            var jobResponse = await _httpClient.GetAsync(endpoint);
                            var jobContent = await jobResponse.Content.ReadAsStringAsync();
                            var job = JsonConvert.DeserializeObject<JobPosting>(jobContent);
                            if (job != null)
                            {
                                jobs.Add(job);
                            }
                        }
                        jobs.Reverse();
                        return View(jobs);
                    }
                    else
                    {
                        Console.WriteLine("Error in receiving user jobs");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("HTTP request error: " + ex.Message);
                    return View("Error");
                }
            }
            else
            {
                return RedirectToAction("SignupEmployee", "Employee");
            }
            return View();
        }

        private async Task<HttpResponseMessage> AddResumeAsync(string userEmail, IFormFile resume, string baseUrl)
        {

            using (var ms = new MemoryStream())
            {

                await resume.CopyToAsync(ms);

                var resumeContent = new ByteArrayContent(ms.ToArray());
                resumeContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                var formData = new MultipartFormDataContent();
                formData.Add(resumeContent, "file", resume.FileName);
                formData.Add(new StringContent(userEmail), "userEmail");

                string encryptedToken = _encryptionService.EncryptToken(TokenManager.TokenString);
                // Sending token in the headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", encryptedToken);

                return await _httpClient.PostAsync(baseUrl + "api/resume", formData);
            }
        }

        // Method to save job application
        private async Task<HttpResponseMessage> SaveJobApplicationAsync(int jobId, string applier, string userEmail, string phoneNumber, int addedResume, int experience, string baseUrl)
        {
            var jobApplication = new JobApplication
            {
                Applier = applier,
                UserEmail = userEmail,
                JobId = jobId,
                PhoneNumber = phoneNumber,
                ResumeId = addedResume,
                Experience = experience
            };
            string encryptedToken = _encryptionService.EncryptToken(TokenManager.TokenString);

            jobApplication.UserEmail = Request.Cookies["Username"];
            string apiEndpoint = "api/JobApplication";

            var jsonContent = new StringContent(JsonConvert.SerializeObject(jobApplication), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", encryptedToken);

            return await _httpClient.PostAsync(apiEndpoint, jsonContent);
        }

        public async Task<IActionResult> Withdraw(int jobId)
        {
            string username = Request.Cookies["Username"];

            try
            {
                string encryptedToken = _encryptionService.EncryptToken(TokenManager.TokenString);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", encryptedToken);

                string endpoint = $"https://localhost:7025/api/JobApplication/{jobId}/{username}";

                var response = await _httpClient.DeleteAsync(endpoint);

                return RedirectToAction("MyJob", "Jobapplication");
            }
            catch (HttpRequestException ex)
            {
                return View("Error");
            }
    
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
