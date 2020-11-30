using System.Text.Json.Serialization;

namespace TimeclockBot.Models
{
    class Employee
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("pis")]
        public string PIS { get; set; }

        [JsonPropertyName("company")]
        public Company Company { get; set; }
    }
}