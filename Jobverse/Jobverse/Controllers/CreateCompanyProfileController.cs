using Microsoft.AspNetCore.Mvc;
using Jobverse.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace Jobverse.Controllers
{
    public class CreateCompanyProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateCompanyProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult CreateCompanyProfile()
        {
            return View("~/Views/CompanyProfile/CreateCompanyProfile.cshtml");
        }
        public IActionResult SignupEmployer()
        {
            return View("~/Views/CompanyProfile/SignupEmployer.cshtml");
        }
        public IActionResult LoginEmployer()
        {
            return View("~/Views/CompanyProfile/LoginEmployer.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> SignupSuccess(Models.Company company)
        {
            //Console.WriteLine($"Company Email is{company.Email} ");
            try
            {
                var apiUrl = "https://localhost:7105/api/Company";
                var result = await PostToApiAsync(apiUrl, company);
                if (result.StatusCode == HttpStatusCode.Conflict)
                {
                    ViewBag.ErrorMessage = "Company with the same email already exists.";
                    return View("~/Views/CompanyProfile/SignupEmployer.cshtml");
                }


                Console.WriteLine(result);

                return View("~/Views/CompanyProfile/CreateCompanyProfile.cshtml");
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                return StatusCode(500, "Internal Server Error");
            }
        }

        private async Task<(string Result, HttpStatusCode StatusCode)> PostToApiAsync(string apiUrl, object data)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(apiUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            return (result, response.StatusCode);
        }
    }
}
