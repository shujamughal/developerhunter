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
using Jobverse.Utils;
using System.Net.Http.Headers;

namespace Jobverse.Controllers
{
    public class EmployerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployerController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult SignupEmployer()
        {
            return View("~/Views/Authentication/Signup.cshtml");
        }
        public IActionResult LoginEmployer()
        {
            return View("~/Views/Authentication/Login.cshtml");
        }
        public IActionResult LogoutEmployer()
        {
            ViewBag.ShowSignup = true;
            Response.Cookies.Delete("Username");
            Response.Cookies.Delete("Company");
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> SigninSuccess(Jobverse.Models.Authentication.Signup.RegisterCompany employer)
        {
            Console.WriteLine($"Yes here i am {employer.Email}");
            try
            {
                var apiUrl = "https://localhost:7105/api/Authentication/Register";
                var result = await PostToApiAsyncSignup(apiUrl, employer);
                var code = (int)result;
                if (code == 200)
                {
                    ViewBag.ShowSignup = true;
                    Response.Cookies.Delete("Username");
                    Console.WriteLine("Company registered successfully\n");
                    return View("~/Views/Authentication/Login.cshtml");
                }
                else if (code == 409)
                {
                    ViewBag.EmailAlreadyExists = "Email is already registered. Please login or use a different email.";
                }
                return View("~/Views/Authentication/Signup.cshtml");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
        private async Task<HttpStatusCode> PostToApiAsyncSignup(string apiUrl, Jobverse.Models.Authentication.Signup.RegisterCompany data)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            Console.WriteLine($"the returned code is {(int)response.StatusCode}");
            return (response.StatusCode);
        }
        public async Task<IActionResult> LoginSuccess(Jobverse.Models.Authentication.Login.LoginCompany employer)
        {
            Console.WriteLine($"Yes I am {employer.Email}");
            try
            {
                var apiUrl = "https://localhost:7105/api/Authentication/Login";
                var result = await PostToApiAsyncSignin(apiUrl, employer);
                var code = (int)result;
                if (code == 200)
                {
                    ViewBag.ShowSignup = true;
                    Response.Cookies.Delete("Username");
                    Console.WriteLine("Company LoggedIn successfully\n");
                    return RedirectToAction("JobsPosted", "Employer");
                }
                else if (code == 409)
                {
                    ViewBag.InvalidCredentials = "Invalid Login Credentials!!";
                    return View("~/Views/Authentication/Login.cshtml");
                }
                return View("~/Views/Authentication/Signup.cshtml");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }

        }
        private async Task<HttpStatusCode> PostToApiAsyncSignin(string apiUrl, Jobverse.Models.Authentication.Login.LoginCompany data)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            string responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
            string token = responseObject.token;
            CompanyTokenManager.CompanyTokenString = token;
            return (response.StatusCode);
        }
        public async Task<IActionResult> JobsPosted()
        {
            try
            {
                var (CompanyEmail, CompanyName) = CompanyTokenManager.getCredentials();

                // Sending company token in the headers
                string tokenString = CompanyTokenManager.CompanyTokenString;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                string endpoint = $"https://localhost:7199/api/JobPosting/{CompanyName}/{1}";

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

        public (string email, string name) GetUserCookie()
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext.Request.Cookies.TryGetValue("Company", out var cookieValue))
                {
                    var cookieValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieValue);

                    if (cookieValues != null &&
                        cookieValues.TryGetValue("Email", out var email) &&
                        cookieValues.TryGetValue("CompanyName", out var name))
                    {
                        return (email, name);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                Console.WriteLine($"Error retrieving cookie: {ex.Message}");
            }

            return (null, null);
        }
        public async Task<IActionResult> JobApplications(int jobId)
        {
            // Sending company token in the headers
            string tokenString = CompanyTokenManager.CompanyTokenString;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

            string endpoint = $"https://localhost:7025/api/JobApplication/{jobId}/c?_=c";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jobApplications = JsonConvert.DeserializeObject<List<JobApplication>>(content);
                if (jobApplications != null)
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
        public async Task<IActionResult> DownloadResume(int jobID, int ResumeId)
        {
            try
            {
                // Sending company token in the headers
                string tokenString = CompanyTokenManager.CompanyTokenString;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

               // string resumeEndpoint = $"https://localhost:7142/api/resume/{ResumeId}";

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