using FluentValidation;
using MetricsHub.Application.Exceptions;
using MetricsHub.Application.Modules.Events.DTOs;
using MetricsHub.Application.Modules.Events.Services.Contracts;
using MetricsHub.Application.Modules.Events.Services.Interfaces;
using MetricsHub.Application.Modules.Events.Services.Mappings;
using System.Text.Json;

namespace MetricsHub.Application.Modules.Events.Services
{
    public class IngestionEventService(IValidator<PulsePayload> pulseValidator, IValidator<AlertPayload> alertValidator, IIngestionEventRepository ingestionEventRepository) : IIngestionEventService
    {
        private readonly IValidator<PulsePayload> pulseValidator = pulseValidator;
        private readonly IValidator<AlertPayload> alertValidator = alertValidator;
        private readonly IIngestionEventRepository ingestionEventRepository = ingestionEventRepository;

        public async Task Ingest(AlertPayload alertPayload)
        {
            await IngestInternal(
                alertPayload.AlertId,
                alertPayload,
                alertValidator,
                payload => payload.ToEvent()
            );
        }

        public async Task Ingest(PulsePayload pulsePayload)
        {
            await IngestInternal(
                pulsePayload.PulseId,
                pulsePayload,
                pulseValidator,
                payload => payload.ToEvent()
            );
        }

        private async Task IngestInternal<TPayload>(
            string id,
            TPayload payload,
            IValidator<TPayload> validator,
            Func<TPayload, IngestionEventRequest> mapper)
        {
            try
            {
                var exists = await ingestionEventRepository.ExistsAsync(id);

                if (exists)
                {
                    throw new DuplicateEventException(id);
                }

                var result = await validator.ValidateAsync(payload);

                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    throw new ApplicationValidationException(errors);
                }

                var eventIngestion = mapper(payload);

                await ingestionEventRepository.AddAsync(eventIngestion);
            }
            catch (DuplicateEventException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException(JsonSerializer.Serialize(payload), ex);
            }
        }
    }
}
