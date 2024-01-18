using ApplyForJob.Models;
using Jobverse.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jobverse.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly HttpClient _httpClient;

        public JobApplicationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5141/");
            //7025
        }

        public async Task<IActionResult> JobApplication()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveApplication([FromBody] JobApplication jobApplication)
        {
            try
            {
                string apiEndpoint = "api/JobApplication";

                // Serialize the FormDataModel to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(jobApplication), Encoding.UTF8, "application/json");

                // Make a POST request to the API endpoint
                var response = await _httpClient.PostAsync(apiEndpoint, jsonContent);
                Console.WriteLine("...1.2");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("...1");
                    // Optionally, you can process the response from the API
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    // You can use apiResponse as needed

                    return Json(new { success = true, message = "Application saved successfully" });
                }
                else
                {
                    Console.WriteLine("...2");
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

        public IActionResult Success()
        {
            return View();
        }
    }
}
