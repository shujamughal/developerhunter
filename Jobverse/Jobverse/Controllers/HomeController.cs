﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; // Make sure to add this using statement

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var apiEndpoint = "https://localhost:7222/api/jobsapi"; // Replace with your actual API endpoint

        using (var client = _httpClientFactory.CreateClient())
        {
            var response = await client.GetAsync(apiEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var jobPostings = JsonConvert.DeserializeObject<List<Jobs>>(content);
                // Console.WriteLine("Usama");
                //Console.WriteLine(jobPostings[0].Location);
                return View(jobPostings);
            }
            else
            {
                // Handle error if needed
                return View("Error");
            }
        }
    }
}
