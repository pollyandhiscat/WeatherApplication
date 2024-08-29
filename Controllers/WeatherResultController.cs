using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherResult.Models;
using Azure.ResourceManager;
using Azure.ResourceManager.Maps;
using Azure.ResourceManager.Maps.Models;
using Azure.Maps.Rendering;
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

/*
 * Code Citations:
 * Citation #18 
 * Citation #19
 * Citation #25
 * Citation #26
 * Citation #31
 * Citation #32
 * Citation #33
 * Citation #38
 * Citation #39
 * Citation #40
 * Citation #41
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
            var queryParam = "";
            var weatherAPIHTTPClient = _httpClientFactory.CreateClient("weatherAPIClient");
            var WeatherAPIKey = "";
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "PRODUCTION")
            {
                // If the environment is production (Azure), we use the environment variable from Azure app service.
                WeatherAPIKey = Environment.GetEnvironmentVariable("WeatherAPIKey");
            }

            else
            {
                // If the environment is development (local), we use the value from our 'secrets.json' file.
                // 'secrets.json' is not stored on GitHub nor part of the Git tracking mechanism.
                WeatherAPIKey = _config.GetValue<string>("WeatherAPIKey");

            }

            if (city is null && state is null && zipcode is null)
            {

                /*
                 * User did not supply anything. This shouldn't happen
                 * with proper HTML form validation, but it exists
                 * as a safety precaution.
                */

                ViewBag.result = "No valid city, state, or zip code was provided. Please navigate back to home and try again.";
                ViewBag.success = false;
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

            else if (city is not null && state is not null & zipcode is null)
            {
                /*
                 * When the user omits the zip code but
                 * provides us the city and state.
                */

                queryParam = $"{city},{state}";

            }

            var response = await weatherAPIHTTPClient.GetAsync($"current.json?key={WeatherAPIKey}&q={queryParam}&aqi=no");

            if (response.IsSuccessStatusCode)
            {
                var responseObject = await response.Content.ReadAsStringAsync();
                var weatherDataSet = JsonSerializer.Deserialize<WeatherData>(responseObject);
                var cityResult = weatherDataSet.location.name;
                var stateResult = weatherDataSet.location.region;
                var countryResult = weatherDataSet.location.country;
                var latitudeResult = weatherDataSet.location.lat;
                var longitudeResult = weatherDataSet.location.lon;
                var timeZoneResult = weatherDataSet.location.tz_id;
                var localTimeResult = weatherDataSet.location.localtime;
                var last_updated_epoch = weatherDataSet.current.last_updated_epoch;
                var last_updated = weatherDataSet.current.last_updated;
                var temp_c = weatherDataSet.current.temp_c;
                var temp_f = weatherDataSet.current.temp_f;
                var is_day = weatherDataSet.current.is_day;
                var condition = weatherDataSet.current.condition;
                var wind_mph = weatherDataSet.current.wind_mph;
                var wind_kph = weatherDataSet.current.wind_kph;
                var wind_degree = weatherDataSet.current.wind_degree;
                var wind_dir = weatherDataSet.current.wind_dir;
                var pressure_mb = weatherDataSet.current.pressure_mb;
                var pressure_in = weatherDataSet.current.pressure_in;
                var precip_mm = weatherDataSet.current.precip_mm;
                var precip_in = weatherDataSet.current.precip_in;
                var humidity = weatherDataSet.current.humidity;
                var cloud = weatherDataSet.current.cloud;
                var feelslike_c = weatherDataSet.current.feelslike_c;
                var feelslike_f = weatherDataSet.current.feelslike_f;
                var windchill_c = weatherDataSet.current.windchill_c;
                var windchill_f = weatherDataSet.current.windchill_f;
                var heatindex_c = weatherDataSet.current.heatindex_c;
                var heatindex_f = weatherDataSet.current.heatindex_f;
                var dewpoint_c = weatherDataSet.current.dewpoint_c;
                var dewpoint_f = weatherDataSet.current.dewpoint_f;
                var vis_km = weatherDataSet.current.vis_km;
                var vis_miles = weatherDataSet.current.vis_miles;
                var uv = weatherDataSet.current.uv;
                var gust_mph = weatherDataSet.current.gust_mph;
                var gust_kph = weatherDataSet.current.gust_kph;
                var icon = weatherDataSet.current.condition.icon;


                ViewBag.name = weatherDataSet.location.name;
                ViewBag.region = weatherDataSet.location.region;
                ViewBag.country = weatherDataSet.location.country;
                ViewBag.lat = weatherDataSet.location.lat;
                ViewBag.lon = weatherDataSet.location.lon;
                ViewBag.tz_id = weatherDataSet.location.tz_id;
                ViewBag.localtime = weatherDataSet.location.localtime;
                ViewBag.last_updated_epoch = last_updated_epoch;
                ViewBag.last_updated = last_updated;
                ViewBag.temp_c = temp_c;
                ViewBag.temp_f = temp_f;
                ViewBag.is_day = is_day;
                ViewBag.condition = condition;
                ViewBag.wind_mph = wind_mph;
                ViewBag.wind_kph = wind_kph;
                ViewBag.wind_degree = wind_degree;
                ViewBag.wind_dir = wind_dir;
                ViewBag.pressure_mb = pressure_mb;
                ViewBag.pressure_in = pressure_in;
                ViewBag.precip_mm = precip_mm;
                ViewBag.precip_in = precip_in;
                ViewBag.humidity = humidity;
                ViewBag.cloud = cloud;
                ViewBag.feelslike_c = feelslike_c;
                ViewBag.feelslike_f = feelslike_f;
                ViewBag.windchill_c = windchill_c;
                ViewBag.windchill_f = windchill_f;
                ViewBag.heatindex_c = heatindex_c;
                ViewBag.heatindex_f = heatindex_f;
                ViewBag.dewpoint_c = dewpoint_c;
                ViewBag.dewpoint_f = dewpoint_f;
                ViewBag.vis_km = vis_km;
                ViewBag.vis_miles = vis_miles;
                ViewBag.uv = uv;
                ViewBag.gust_mph = gust_mph;
                ViewBag.gust_kph = gust_kph;
                ViewBag.icon = icon;
                ViewBag.success = true;

                return View("WeatherResult");

    }

            else
            {
                ViewBag.success = false;
                ViewBag.result = $"There was an error making the API call. \n Response: {response.ReasonPhrase}";
                return View("WeatherResult");
}
        }
    }
}
