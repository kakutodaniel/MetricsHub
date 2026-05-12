using MetricsHub.Application.Modules.Events.DTOs;

namespace MetricsHub.Application.Modules.Events.Services.Interfaces
{
    public interface IIngestionEventService
    {
        Task Ingest(AlertPayload alertPayload);

        Task Ingest(PulsePayload pulsePayload);
    }
}
