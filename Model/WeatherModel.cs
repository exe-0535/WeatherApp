using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Model
{
    public class WeatherModel
    {

        public Coord coord { get; set; }
        public Weather[] weather { get; set; }
        public string _base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public string timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }


        public class Coord
        {
            public float lon { get; set; }
            public float lat { get; set; }
        }

        public class Main
        {
            public string temp { get; set; }
            public string feels_like { get; set; }
            public string temp_min { get; set; }
            public string temp_max { get; set; }
            public string pressure { get; set; }
            public string humidity { get; set; }
        }

        public class Wind
        {
            public string speed { get; set; }
            public string deg { get; set; }
        }

        public class Clouds
        {
            public int all { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public string country { get; set; }
            public string sunrise { get; set; }
            public string sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

    }
}
