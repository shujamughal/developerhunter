﻿using Microsoft.AspNetCore.Mvc;
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
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Jobverse.Utils;

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
        /// <summary>
        /// abdulrehman
        /// </summary>
        /// <param name="employer"></param>
        /// <returns></returns>
        public async Task<IActionResult> SigninSuccess(Jobverse.Models.Authentication.Signup.RegisterCompany employer)
        {
            Console.WriteLine($"Yes here i am {employer.Email}");
            try
            {
                var apiUrl = "https://localhost:7105/api/Authentication/Register";
                var result = await PostToApiAsyncSignup(apiUrl, employer);
                var code=(int)result;
                if(code==200)
                {
                    Console.WriteLine("User registered successfully\n");
                    return RedirectToAction("JobsPosted", "Employer");
                }
                else if(code==409)
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
            return (response.StatusCode);
        }
        public async Task<IActionResult> LoginSuccess(Jobverse.Models.Authentication.Login.LoginCompany employer)
        {
            Console.WriteLine($"Yes here i am {employer.Email}");
            try
            {
                var apiUrl = "https://localhost:7105/api/Authentication/Login";
                var result = await PostToApiAsyncSignin(apiUrl, employer);
                var code = (int)result;
                if (code == 200)
                {
                    Console.WriteLine("User LogedIn successfully\n");
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
            TokenManager.CompanyToken = token;
            return (response.StatusCode);
        }
        /// <summary>
        /// abdulrehman'ss
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> JobsPosted()
        {
            try
            {
                var (CompanyEmail, CompanyName) = TokenManager.getCredentials();
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
        private (string email,string name) getCookieValues()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext.Request.Cookies.TryGetValue("Company", out var cookieValue))
            {
                var cookieValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieValue);
                Console.WriteLine(cookieValue);
                var email = cookieValues.ContainsKey("Email") ? cookieValues["Email"] : string.Empty;
                var companyName = cookieValues.ContainsKey("CompanyName") ? cookieValues["CompanyName"] : string.Empty;
                Console.WriteLine($"Email: {email}, CompanyName: {companyName}");

                return (email, companyName);
            }

            Console.WriteLine("Cookie not found");
            return (string.Empty,string.Empty);
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
    }
}
