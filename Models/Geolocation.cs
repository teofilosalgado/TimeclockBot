using System.Text.Json.Serialization;

namespace TimeclockBot.Models
{
    class Geolocation
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("dateAndTime")]
        public string DateAndTime { get; set; }
    }
}