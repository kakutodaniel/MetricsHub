using MetricsHub.Application.Exceptions;
using MetricsHub.Application.Modules.Events.Services.Contracts;
using MetricsHub.Application.Modules.Events.Services.Interfaces;
using MetricsHub.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MetricsHub.Infrastructure.Persistence.Repositories
{
    public class IngestionEventRepository(MetricsHubDbContext metricsHubDbContext) : IIngestionEventRepository
    {
        private readonly MetricsHubDbContext metricsHubDbContext = metricsHubDbContext;

        public async Task AddAsync(IngestionEventRequest ingestionEvent)
        {
            var entity = ingestionEvent.ToEntity();

            await metricsHubDbContext.AddAsync(entity);

            try
            {
                await metricsHubDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is Microsoft.Data.Sqlite.SqliteException sqliteEx &&
                    sqliteEx.SqliteErrorCode == 19) // constraint violation
                {
                    throw new DuplicateEventException(ingestionEvent.EventId);
                }

                throw;
            }
        }

        public async Task<bool> ExistsAsync(string eventId)
        {
            return await metricsHubDbContext.IngestionEvents
                .AsNoTracking()
                .AnyAsync(x => x.EventId == eventId);
        }
    }
}
