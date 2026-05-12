using MetricsHub.Application.Pagination;

namespace MetricsHub.Application.Modules.Events.Queries.Contracts
{
    public class IngestionEventFilter
    {
        public string? EventId { get; set; }
        public string? SourceSystem { get; set; }
        public string? ResourceName { get; set; }
        public TimeRange? TimeRange { get; set; }
        public bool? IncludeRelated { get; set; }
        public PaginationRequest Pagination { get; set; } = new();
    }
}
