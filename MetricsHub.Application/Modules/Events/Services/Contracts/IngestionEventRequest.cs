namespace MetricsHub.Application.Modules.Events.Services.Contracts
{
    public class IngestionEventRequest
    {
        public string EventId { get; set; }
        public string Source { get; set; }
        public DateTime Timestamp { get; set; }
        public string ResourceName { get; set; }
        public string Region { get; set; }
        public Dictionary<string, double>? Metrics { get; set; }
        public Dictionary<string, object>? Attributes { get; set; }
        public string? CorrelationId { get; set; }
    }
}
