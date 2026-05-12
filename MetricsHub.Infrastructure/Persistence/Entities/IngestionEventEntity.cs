namespace MetricsHub.Infrastructure.Persistence.Entities
{
    public class IngestionEventEntity
    {
        public required string EventId { get; set; }
        public required string Source { get; set; }
        public DateTime Timestamp { get; set; }
        public required string ResourceName { get; set; }
        public required string Region { get; set; }
        public string? MetricsJson { get; set; }
        public string? AttributesJson { get; set; }
        public string? CorrelationId { get; set; }
    }
}
