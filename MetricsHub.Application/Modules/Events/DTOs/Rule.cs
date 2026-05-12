using System.Text.Json.Serialization;

namespace MetricsHub.Application.Modules.Events.DTOs
{
    public record Rule(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("threshold")] double Threshold,
        [property: JsonPropertyName("actual")] double Actual,
        [property: JsonPropertyName("metric")] string Metric,
        [property: JsonPropertyName("severity")] string Severity
    );
}
