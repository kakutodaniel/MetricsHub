using MetricsHub.Application.Modules.Events.Queries.Contracts;
using MetricsHub.Application.Modules.Events.Services.Contracts;
using MetricsHub.Infrastructure.Persistence.Entities;
using System.Text.Json;

namespace MetricsHub.Infrastructure.Persistence.Mappings
{
    public static class IngestionEventMapper
    {
        public static IngestionEventEntity ToEntity(this IngestionEventRequest e)
        {
            return new IngestionEventEntity
            {
                EventId = e.EventId,
                Source = e.Source,
                Timestamp = e.Timestamp,
                ResourceName = e.ResourceName,
                Region = e.Region,
                CorrelationId = e.CorrelationId,
                MetricsJson = e.Metrics == null ? null : JsonSerializer.Serialize(e.Metrics),
                AttributesJson = e.Attributes == null ? null : JsonSerializer.Serialize(e.Attributes)
            };
        }

        public static IngestionEventQueryResponse ToEvent(this IngestionEventEntity e)
        {
            return new IngestionEventQueryResponse
            {
                EventId = e.EventId,
                Source = e.Source,
                Timestamp = e.Timestamp,
                ResourceName = e.ResourceName,
                Region = e.Region,
                CorrelationId = e.CorrelationId,
                Metrics = e.MetricsJson == null ? null : JsonSerializer.Deserialize<Dictionary<string, double>>(e.MetricsJson),
                Attributes = e.AttributesJson == null ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(e.AttributesJson)
            };
        }
    }
}
