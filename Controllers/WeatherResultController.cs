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
        public async Task<IActionResult> ShowResult(string city, string state, int zip)
        {

            var httpClient = _httpClientFactory.CreateClient("weatherAPIClient");
            var queryParam = "";
            var key = _config.GetValue<string>("WeatherAPIKey");
            List<WeatherResultModel> weatherList = new List<WeatherResultModel>();

            if (city is null && state is null && zip == 0) {

                /*
                 * User did not supply anything. This shouldn't happen
                 * with proper HTML form validation, but it exists
                 * as a safety precaution.
                */

                ViewBag.result = "No valid city, state, or zip code was provided";
                return View();

            }

            else if (city != null && state is null & zip == 0) {

                // If the user only supplied the city.
                queryParam = $"{city}";
            
            }

            else if (zip is not 0) {

                // User did not specify a zip code. Use city value instead.
                queryParam = $"{zip}";
            }

            else
            {

                queryParam = $"{zip}";
                // User provided all params and we can formulate the most precise query call.
            }

            var response = await httpClient.GetAsync($"current.json?key={key}&q={queryParam}&aqi=no");

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await response.Content.ReadAsStringAsync();
                var responseList = JsonSerializer.Deserialize<List<WeatherResultModel>>(responseObject);
                ViewBag.result = responseList.ToString();
                return View("WeatherResult");

            }

            else
            {
                ViewBag.result = $"There was an error making the API call. Return code: {response.StatusCode}";
                return View("WeatherResult");
            }
        }
    }
}
