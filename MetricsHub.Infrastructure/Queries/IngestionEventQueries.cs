using MetricsHub.Application.Modules.Events.Queries;
using MetricsHub.Application.Modules.Events.Queries.Contracts;
using MetricsHub.Application.Pagination;
using MetricsHub.Infrastructure.Persistence;
using MetricsHub.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MetricsHub.Infrastructure.Queries
{
    public class IngestionEventQueries(MetricsHubDbContext metricsHubDbContext) : IIngestionEventQueries
    {
        private readonly MetricsHubDbContext metricsHubDbContext = metricsHubDbContext;

        public async Task<PaginatedResult<IngestionEventQueryResponse>> GetAsync(IngestionEventFilter filter)
        {
            var q = metricsHubDbContext.IngestionEvents.AsQueryable();

            // Filter: eventId
            if (!string.IsNullOrWhiteSpace(filter.EventId))
            {
                q = q.Where(x => x.EventId == filter.EventId);
            }

            // Filter: source system
            if (!string.IsNullOrWhiteSpace(filter.SourceSystem))
            {
                q = q.Where(x => x.Source == filter.SourceSystem.ToLowerInvariant());
            }

            // Filter: resource name
            if (!string.IsNullOrWhiteSpace(filter.ResourceName))
            {
                q = q.Where(x => x.ResourceName == filter.ResourceName.ToLowerInvariant());
            }

            // Filter: time range
            if (filter.TimeRange != null)
            {
                if (filter.TimeRange.From.HasValue)
                {
                    q = q.Where(x => x.Timestamp >= filter.TimeRange.From.Value);
                }

                if (filter.TimeRange.To.HasValue)
                {
                    q = q.Where(x => x.Timestamp <= filter.TimeRange.To.Value);
                }
            }

            // Total count BEFORE pagination
            var totalCount = await q.CountAsync();

            // Apply pagination
            var items = await q
                .OrderByDescending(x => x.Timestamp)
                .Skip(filter.Pagination.Skip)
                .Take(filter.Pagination.PageSize)
                .Select(x => x.ToEvent())
                .ToListAsync();

            // apply correlation
            // TODO: MUST BE ONLY FOR SPECIFIC EVENTID
            if (!string.IsNullOrWhiteSpace(filter.EventId) && filter.IncludeRelated == true)
            {
                var eventIds = items.Select(x => x.EventId);
                var correlations = await metricsHubDbContext.IngestionEvents.AsQueryable().Where(x => eventIds.Contains(x.CorrelationId)).ToListAsync();

                var lookup = correlations
                    .Where(x => x.CorrelationId != null)
                    .GroupBy(x => x.CorrelationId!)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ToEvent()).ToList()
                    );

                foreach (var item in items)
                {
                    item.Related = lookup.TryGetValue(item.EventId, out var rel)
                        ? rel
                        : null;
                }
            }

            // Build result
            return PaginatedResult<IngestionEventQueryResponse>.Create(
                items,
                totalCount,
                filter.Pagination.PageNumber,
                filter.Pagination.PageSize);
        }
    }
}
