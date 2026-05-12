using System.Text.Json.Serialization;

namespace MetricsHub.Application.Modules.Events.DTOs
{
    public record Resource(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("group")] string Group,
        [property: JsonPropertyName("environment")] string Environment
    );
}
