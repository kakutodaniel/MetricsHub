using System.Text.Json.Serialization;

namespace MetricsHub.Application.Modules.Events.DTOs
{
    public record PulsePayload(
        [property: JsonPropertyName("pulse_id")] string PulseId,
        [property: JsonPropertyName("ts")] string Ts,
        [property: JsonPropertyName("node")] string Node,
        [property: JsonPropertyName("region")] string Region,
        [property: JsonPropertyName("metrics")] Metrics Metrics,
        [property: JsonPropertyName("tags")] List<string> Tags
    );
}
