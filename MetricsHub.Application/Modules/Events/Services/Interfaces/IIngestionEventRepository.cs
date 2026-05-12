using MetricsHub.Application.Modules.Events.Services.Contracts;

namespace MetricsHub.Application.Modules.Events.Services.Interfaces
{
    public interface IIngestionEventRepository
    {
        Task AddAsync(IngestionEventRequest ingestionEvent);

        Task<bool> ExistsAsync(string eventId);
    }
}
