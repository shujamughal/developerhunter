using jobPosting.Models;
using Jobverse.Models;
using Jobverse.Services;
using Jobverse.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jobverse.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenEncryptionService _encryptionService;

        public JobPostingController(IHttpClientFactory httpClientFactory, ITokenEncryptionService encryptionService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7199/");
            _encryptionService = encryptionService;
        }

        public async Task<IActionResult> JobPosting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostJob(JobPosting jobPosting)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string tokenString = CompanyTokenManager.CompanyTokenString;

                    string apiEndpoint = "api/JobPosting";

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(jobPosting), Encoding.UTF8, "application/json");

                    // Sending company token in the headers
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                    // Make a POST request to the API endpoint
                    var response = await _httpClient.PostAsync(apiEndpoint, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStringAsync();
						return RedirectToAction("JobsPosted", "Employer");
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error in API request" });
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("HTTP request error: " + ex.Message);

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    }

                    return Json(new { success = false, message = "Error in HTTP request" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Model validation failed" });
            }
        }

        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                string tokenString = _encryptionService.EncryptToken(CompanyTokenManager.CompanyTokenString);
                string apiEndpoint = $"api/JobPosting/{id}";

                // Sending token in the headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                // Make a POST request to the API endpoint
                var response = await _httpClient.DeleteAsync(apiEndpoint);

                return RedirectToAction("JobsPosted", "Employer");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP request error: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }

                return Json(new { success = false, message = "Error in HTTP request" });
            }
        }
        public async Task<IActionResult> Disable(int jobId)
        {
            try
            {
                string tokenString = CompanyTokenManager.CompanyTokenString;
                // Sending company token in the headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                string endpoint = $"https://localhost:7199/api/JobPosting/{jobId}";
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();
                var job = JsonConvert.DeserializeObject<JobPosting>(content);


                if(job != null && job.Enabled == true)
                {
                    job.Enabled = false;
                }
                else
                {
                    job.Enabled = true;
                }

                var jsonContent = new StringContent(JsonConvert.SerializeObject(job), Encoding.UTF8, "application/json");

                // Sending company token in the headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);

                endpoint = $"https://localhost:7199/api/JobPosting/{jobId}";

                await _httpClient.PutAsync(endpoint, jsonContent);

                return RedirectToAction("JobsPosted", "Employer");
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
