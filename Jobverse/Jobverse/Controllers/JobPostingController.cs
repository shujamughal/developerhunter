using jobPosting.Models;
using Jobverse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jobverse.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly HttpClient _httpClient;

        public JobPostingController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7199/");
        }

        public async Task<IActionResult> JobPosting(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostJob(JobPosting jobPosting)
        {
            Console.WriteLine(jobPosting.Email);
            if (ModelState.IsValid) // Checks if the model binding was successful
            {
                if (string.IsNullOrWhiteSpace(jobPosting.JobTitle) ||
                    string.IsNullOrWhiteSpace(jobPosting.JobDescription) ||
                    string.IsNullOrWhiteSpace(jobPosting.Location) ||
                    string.IsNullOrWhiteSpace(jobPosting.Type) ||
                    string.IsNullOrWhiteSpace(jobPosting.SalaryRange) ||
                    string.IsNullOrWhiteSpace(jobPosting.Qualifications) ||
                    jobPosting.PostedDate == DateTime.Now ||
                    jobPosting.LastDate == DateTime.MinValue)
                {
                    return Json(new { success = false, message = "Please provide all required fields" });
                }
                else
                {
                    try
                    {
                        string apiEndpoint = "api/JobPosting";

                        // Serialize the FormDataModel to JSON
                        var jsonContent = new StringContent(JsonConvert.SerializeObject(jobPosting), Encoding.UTF8, "application/json");

                        // Make a POST request to the API endpoint
                        var response = await _httpClient.PostAsync(apiEndpoint, jsonContent);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)
                        {
                            // Optionally, you can process the response from the API
                            var apiResponse = await response.Content.ReadAsStringAsync();
                            // You can use apiResponse as needed

                            return Json(new { success = true, message = "Job Posted successfully" });
                        }
                        else
                        {
                            // Handle the case when the API request is not successful
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
            }
            else
            {
                return Json(new { success = false, message = "Model validation failed" });
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
