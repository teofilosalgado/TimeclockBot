using System.Collections.Generic;
using System.Text.Json.Serialization;
using TimeclockBot.Models;

namespace TimeclockBot.Serializers
{
    class ClockingSerializer
    {
        [JsonPropertyName("clockingEvents")]
        public List<Clocking> ClockingEvents { get; set; }
    }
}