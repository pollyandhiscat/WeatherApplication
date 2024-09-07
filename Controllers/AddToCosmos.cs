using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using WeatherResult.Models;
using System.Diagnostics.Metrics;
using WeatherApplication.Models;
using HistoricalInformation.Models;

namespace WeatherApplication.Controllers
{
    public class AddToCosmos : Controller
    {

        public static async Task UpdateCosmos(string cosmosDBEndpoint, string cosmosDBKey, WeatherData weatherDataSet)
        {

            var databaseName = "WeatherApp";
            var city = weatherDataSet.location.name;
            var state = weatherDataSet.location.region;
            var country = weatherDataSet.location.country;
            var localTime = weatherDataSet.location.localtime;
            var temp_c = weatherDataSet.current.temp_c;
            var temp_f = weatherDataSet.current.temp_f;
            var wind_mph = weatherDataSet.current.wind_mph;
            var wind_kph = weatherDataSet.current.wind_kph;
            var humidity = weatherDataSet.current.humidity;
            var uv = weatherDataSet.current.uv;

            CosmosClient client = new CosmosClient(cosmosDBEndpoint, cosmosDBKey);
            Database database = client.GetDatabase(databaseName);
            Container container = database.GetContainer("HistoricalLocationInformation");


            HistoricalLocationInformationModel locationData = new HistoricalLocationInformationModel();


            locationData.id = Guid.NewGuid().ToString();
            locationData.category = city;
            locationData.country = country;
            locationData.state = state;
            locationData.localtime = localTime;
            locationData.temp_f = temp_f.ToString();
            locationData.temp_c = temp_c.ToString();
            locationData.wind_speed_mph = wind_mph.ToString();
            locationData.wind_speed_kph = wind_kph.ToString();
            locationData.humidity = humidity.ToString();
            locationData.uv_index = uv.ToString();

            await container.UpsertItemAsync(locationData);


        }
    }
}