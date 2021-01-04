using System.Collections.Generic;
using System.Text.Json.Serialization;
using TimeclockBot.Models;

namespace TimeclockBot.Serializers
{
    class ResponseSerializer
    {
        [JsonPropertyName("importResult")]
        public List<Clocking> ImportResult { get; set; }
    }
}