﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
                    jobPostings.Reverse(); // Latest job at the top
                    return View(jobPostings);
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
