using System.Text.Json.Serialization;

namespace TimeclockBot.Models
{
    class Clocking
    {
        [JsonPropertyName("cnpj")]
        public string CNPJ { get; set; }

        [JsonPropertyName("pis")]
        public string PIS { get; set; }

        [JsonPropertyName("appVersion")]
        public string AppVersion { get; set; }

        [JsonPropertyName("employee")]
        public Employee Employee { get; set; }

        [JsonPropertyName("geolocation")]
        public Geolocation Geolocation { get; set; }
    }
}