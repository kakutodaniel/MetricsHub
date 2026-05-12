using MetricsHub.Application.Modules.Events.Queries.Contracts;
using MetricsHub.Application.Pagination;

namespace MetricsHub.Application.Modules.Events.Queries
{
    public interface IIngestionEventQueries
    {
        Task<PaginatedResult<IngestionEventQueryResponse>> GetAsync(IngestionEventFilter filter);
    }
}
