using SQLite;
using System;

namespace ApiClima.Models
{
    public class WeatherData // Cambiado a public
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class WeatherApiResponse
        {
            public Location location { get; set; }
            public Current current { get; set; }
        }

        public class Location
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public string name { get; set; }
            public string localtime { get; set; }
        }

        public class Current
        {
            public double temp_c { get; set; }
            public Condition condition { get; set; }
        }

        public class Condition
        {
            public string text { get; set; }
            public string icon { get; set; }
        }




    }
}
