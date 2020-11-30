using System.Text.Json.Serialization;
using TimeclockBot.Models;

namespace TimeclockBot.Serializer
{
    class EmployeeSerializer
    {
        [JsonPropertyName("employee")]
        public Employee Employee { get; set; }
    }
}