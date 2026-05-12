using System.Text.Json.Serialization;

namespace MetricsHub.Application.Modules.Events.Queries.Contracts
{
    public class IngestionEventQueryResponse
    {
        public string EventId { get; set; }
        public string Source { get; set; }
        public DateTime Timestamp { get; set; }
        public string ResourceName { get; set; }
        public string Region { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, double>? Metrics { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object>? Attributes { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CorrelationId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IEnumerable<IngestionEventQueryResponse>? Related { get; set; }
    }
}
