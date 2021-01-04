using System.Text.Json.Serialization;
using TimeclockBot.Models;

namespace TimeclockBot.Serializers
{
    class EmployeeSerializer
    {
        [JsonPropertyName("employee")]
        public Employee Employee { get; set; }
    }
}