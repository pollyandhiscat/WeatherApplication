using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherResult.Models;

/*
 * Code Citations:
 * Citation #18 
 * Citation #19
 * Citation #25
 * Citation #26
*/

namespace WeatherApplication.Controllers
{
    public class WeatherResult : Controller
    {

        private readonly ILogger<WeatherResult> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public WeatherResult(ILogger<WeatherResult> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _config = configuration;
        }

        public IConfiguration _config;

        [HttpPost]
        public async Task<IActionResult> ShowResult(string city, string state, string zipcode)
        {

            var httpClient = _httpClientFactory.CreateClient("weatherAPIClient");
            var queryParam = "";
            var key = _config.GetValue<string>("WeatherAPIKey");

            if (city is null && state is null && zipcode is null) {

                /*
                 * User did not supply anything. This shouldn't happen
                 * with proper HTML form validation, but it exists
                 * as a safety precaution.
                */

                ViewBag.result = "No valid city, state, or zip code was provided";
                return View("WeatherResult");

            }

            else if (zipcode is not null)
            {

                /*
                 * If the user supplies the zip, we should default to that
                 * as it is the most accurate.
                */

                queryParam = $"{zipcode}";
            }

            else if (city is not null  && state is not null & zipcode is null) {
                /*
                 * When the user omits the zip code but
                 * provides us the city and state.
                */

                queryParam = $"{city},{state}";
            
            }

            var response = await httpClient.GetAsync($"current.json?key={key}&q={queryParam}&aqi=no");

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await response.Content.ReadAsStringAsync();
                ViewBag.result = responseObject;
                return View("WeatherResult");

            }

            else
            {
                ViewBag.result = $"There was an error making the API call. \n Response: {response.ReasonPhrase}";
                return View("WeatherResult");
            }
        }
    }
}
