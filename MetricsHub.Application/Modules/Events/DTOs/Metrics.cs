using System.Text.Json.Serialization;

namespace MetricsHub.Application.Modules.Events.DTOs
{
    public record Metrics(
        [property: JsonPropertyName("cpu_pct")] double CpuPct,
        [property: JsonPropertyName("mem_mb")] int MemMb,
        [property: JsonPropertyName("active_conns")] int ActiveConns,
        [property: JsonPropertyName("status")] string Status
    );
}
