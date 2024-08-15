using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherAPIController : ControllerBase
    {

        [HttpGet("")]

        public async Task<IActionResult> RetrieveWeather(string key, string city, string state, int zip=0)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.weatherapi.com/v1/");

            var queryParam = "";


            if (city is null && state is null && zip == 0) {

                /*
                 * User did not supply anything. This shouldn't happen
                 * with proper HTML form validation, but it exists
                 * as a safety precaution.
                */

                return Problem("No valid city, state, or zip code was provided.");

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



            var response = client.GetAsync($"current.json?key={key}&q={queryParam}&aqi=no"); // TODO: we need to pass a variable in here that contains the search string.
            return Ok();

        }
    }
}
