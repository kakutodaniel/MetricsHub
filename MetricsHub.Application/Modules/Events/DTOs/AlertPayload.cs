using System.Text.Json.Serialization;

namespace MetricsHub.Application.Modules.Events.DTOs
{
    public record AlertPayload(
        [property: JsonPropertyName("alert_id")] string AlertId,
        [property: JsonPropertyName("fired_at")] string FiredAt,
        [property: JsonPropertyName("resource")] Resource Resource,
        [property: JsonPropertyName("rule")] Rule Rule,
        [property: JsonPropertyName("correlation_id")] string CorrelationId
    );
}
