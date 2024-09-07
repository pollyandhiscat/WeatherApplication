namespace HistoricalInformation.Models
{
    public class HistoricalLocationInformationModel
    {

          public string id { get; set; }
          public string category { get; set; }
          public string country { get; set; }
          public string state { get; set; }
          public string localtime { get; set; }
          public string temp_f { get; set; }
          public string temp_c { get; set; }
          public string wind_speed_mph { get; set; }
          public string wind_speed_kph { get; set; }
          public string humidity { get; set; }
          public string uv_index { get; set; }

        }
    }
