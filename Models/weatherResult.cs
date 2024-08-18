/*
 * Code Citations:
 * Citation #27 
 * Citation #28
*/

namespace WeatherResult.Models
{
    public class WeatherResultModel
    {
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }

    }

    public class locationData
    {

        public string Location { get; set; }
        public Dictionary<string, locationDataPoints> location { get; set; }

    }

    public class locationDataPoints
    {

        public string name { get; set; }
        public string region { get; set; }
        public string country { get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public string tz_id { get; set; }
        public string localtime { get; set; }

    }

}
