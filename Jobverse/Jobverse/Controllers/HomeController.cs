using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Jobverse.Models;
using jobPosting.Models;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        // for navbar
        bool showSignup = true;
        bool companyLoggedIn = false;
        if (Request.Cookies.ContainsKey("Username"))
        {
            showSignup = false;
        }
        if (Request.Cookies.ContainsKey("Company"))
        {
            companyLoggedIn = true;
        }

        ViewBag.ShowSignup = showSignup;
        ViewBag.CompanyLoggedIn = companyLoggedIn;
        var apiEndpoint = "https://localhost:7199/api/JobPosting";

        using (var client = _httpClientFactory.CreateClient())
        {
            var response = await client.GetAsync(apiEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var jobPostings = JsonConvert.DeserializeObject<List<JobPosting>>(content);
                if(jobPostings != null)
                {
                    var filteredJobPostings = jobPostings
                        .Where(job => job.Enabled == true).ToList();

                    filteredJobPostings = filteredJobPostings.Where(job => job.LastDate >= DateTime.Today).ToList();

                    filteredJobPostings.Reverse(); // Latest job at the top

                    return View(filteredJobPostings);
                }
                return View(null);
            }
            else
            {
                return View(null);
            }
        }
    }
}
