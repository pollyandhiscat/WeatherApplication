using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;

/*
 * Code Citations:
 * Citation #18 
 * Citation #19
*/

namespace WeatherApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WeatherAPIController : ControllerBase
    {

        private readonly ILogger<WeatherAPIController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public WeatherAPIController(ILogger<WeatherAPIController> logger, IHttpClientFactory httpClientFactory)
        {
            
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IConfiguration _config;
        public WeatherAPIController(IConfiguration configuration) {

            _config = configuration;
        
        }

        public string GetWeatherAPIKey() {

         return _config.GetValue<string>("WeatherAPIKey");

        }

        [HttpGet]

        public async Task<string> RetrieveWeather(string city, string state, int zip=0)
        {

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("http://api.weatherapi.com/v1/");

            var queryParam = "";
            var key = GetWeatherAPIKey();       // TODO: We need to pass in the Azure key vault value to this variable. This is a fake key value.

            if (city is null && state is null && zip == 0) {

                /*
                 * User did not supply anything. This shouldn't happen
                 * with proper HTML form validation, but it exists
                 * as a safety precaution.
                */

                return "No valid city, state, or zip code was provided.";

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

                // User provided all params and we can formulate the most precise query call.
            }

            var response = await httpClient.GetAsync($"current.json?key={key}&q={queryParam}&aqi=no");
            return await response.Content.ReadAsStringAsync();

        }
    }
}
