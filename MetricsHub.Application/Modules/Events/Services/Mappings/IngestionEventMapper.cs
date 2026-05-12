using MetricsHub.Application.Modules.Events.DTOs;
using MetricsHub.Application.Modules.Events.Services.Contracts;

namespace MetricsHub.Application.Modules.Events.Services.Mappings
{
    public static class IngestionEventMapper
    {
        public static IngestionEventRequest ToEvent(this AlertPayload alertPayload)
        {
            return new IngestionEventRequest
            {
                EventId = alertPayload.AlertId,
                Source = "alert",
                Timestamp = DateTimeOffset.Parse(alertPayload.FiredAt).UtcDateTime,
                ResourceName = alertPayload.Resource.Name.ToLowerInvariant(),
                Region = alertPayload.Resource.Group,
                Attributes = new Dictionary<string, object>
                {
                     { nameof(alertPayload.Resource.Environment), alertPayload.Resource.Environment },
                     { nameof(alertPayload.Rule), alertPayload.Rule }
                },
                CorrelationId = alertPayload.CorrelationId,
            };
        }

        public static IngestionEventRequest ToEvent(this PulsePayload pulsePayload)
        {
            return new IngestionEventRequest
            {
                EventId = pulsePayload.PulseId,
                Source = "pulse",
                Timestamp = DateTimeOffset.Parse(pulsePayload.Ts).UtcDateTime,
                ResourceName = pulsePayload.Node.ToLowerInvariant(),
                Region = pulsePayload.Region,
                Metrics = new Dictionary<string, double>
                {
                    { nameof(pulsePayload.Metrics.CpuPct), pulsePayload.Metrics.CpuPct },
                    { nameof(pulsePayload.Metrics.MemMb), pulsePayload.Metrics.MemMb },
                    { nameof(pulsePayload.Metrics.ActiveConns), pulsePayload.Metrics.ActiveConns }
                },
                Attributes = new Dictionary<string, object>
                {
                     { nameof(pulsePayload.Metrics.Status), pulsePayload.Metrics.Status },
                     { nameof(pulsePayload.Tags), pulsePayload.Tags }
                }
            };
        }
    }
}
