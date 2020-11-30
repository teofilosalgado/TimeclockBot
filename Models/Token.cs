using System.Text.Json.Serialization;

namespace TimeclockBot.Models
{
    class Token
    {
        [JsonPropertyName("token_type")]
        public string Type { get; set; }

        [JsonPropertyName("access_token")]
        public string Value { get; set; }
    }
}