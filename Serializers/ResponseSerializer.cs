using System.Collections.Generic;
using System.Text.Json.Serialization;
using TimeclockBot.Models;

namespace TimeclockBot.Serializer
{
    class ResponseSerializer
    {
        [JsonPropertyName("importResult")]
        public List<Clocking> ImportResult { get; set; }
    }
}