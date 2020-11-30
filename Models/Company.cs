using System.Text.Json.Serialization;

namespace TimeclockBot.Models
{
    class Company
    {
        [JsonPropertyName("cnpj")]
        public string CNPJ { get; set; }
    }
}