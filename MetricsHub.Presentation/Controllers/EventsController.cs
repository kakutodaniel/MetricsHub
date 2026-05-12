using MetricsHub.Application.Modules.Events.DTOs;
using MetricsHub.Application.Modules.Events.Queries;
using MetricsHub.Application.Modules.Events.Queries.Contracts;
using MetricsHub.Application.Modules.Events.Services.Interfaces;
using MetricsHub.Application.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Text.Json;

namespace MetricsHub.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(IIngestionEventService eventIngestionService, IIngestionEventQueries ingestionEventQueries) : ControllerBase
    {
        private readonly IIngestionEventService eventIngestionService = eventIngestionService;
        private readonly IIngestionEventQueries ingestionEventQueries = ingestionEventQueries;

        [HttpPost]
        [EnableRateLimiting("LimitByIp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Receive([FromBody] JsonElement body)
        {
            if (!Request.Headers.TryGetValue("X-Source", out var source))
            {
                return BadRequest("Missing X-Source header");
            }

            // TODO: once the app grows we should use factory + strategy patternn
            switch (source.ToString().ToLowerInvariant())
            {
                case "pulse":
                    var pulse = body.Deserialize<PulsePayload>();

                    if (pulse is null)
                    {
                        return BadRequest("Invalid pulse payload");
                    }

                    await eventIngestionService.Ingest(pulse);

                    return Ok();

                case "alert":
                    var alert = body.Deserialize<AlertPayload>();

                    if (alert is null)
                    {
                        return BadRequest("Invalid alert payload");
                    }

                    await eventIngestionService.Ingest(alert);

                    return Ok();

                default:
                    return BadRequest($"Unknown X-Source: {source}");
            }
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(PaginatedResult<IngestionEventQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedResult<IngestionEventQueryResponse>>> Search(
            [FromBody] IngestionEventFilter filter,
            CancellationToken cancellationToken)
        {
            var result = await ingestionEventQueries.GetAsync(filter);
            return Ok(result);
        }
    }
}

