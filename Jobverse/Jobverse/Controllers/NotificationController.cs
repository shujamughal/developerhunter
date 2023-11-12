using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Jobverse.Models;

namespace Jobverse.Controllers
{
    public class NotificationController : Controller
    {
        private readonly HttpClient _httpClient;
        public NotificationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44379/");
        }

        public async Task<IActionResult> Notification()
        {
            var response = await _httpClient.GetAsync("api/notification");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var notifications = JsonConvert.DeserializeObject<List<Notification>>(content);

                return View(notifications);
            }
            else
            {
                // Handle the case where the API request was not successful
                return View("Error");
            }
        }
    }

}
