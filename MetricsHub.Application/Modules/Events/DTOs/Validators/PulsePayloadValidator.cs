using FluentValidation;

namespace MetricsHub.Application.Modules.Events.DTOs.Validators
{
    public class PulsePayloadValidator : AbstractValidator<PulsePayload>
    {
        public PulsePayloadValidator()
        {
            RuleFor(x => x.PulseId)
                .NotEmpty();

            RuleFor(x => x.Ts)
                .BeValidDateTime()
                .WithMessage("Timestamp '{PropertyValue}' is not valid");

            RuleFor(x => x.Node)
                .NotEmpty();

            RuleFor(x => x.Region)
                .NotEmpty();

            RuleFor(x => x.Metrics)
                .NotNull()
                .SetValidator(new MetricsValidator());

            RuleFor(x => x.Tags)
                .NotNull()
                .Must(tags => tags.Count > 0)
                .ForEach(tag =>
                {
                    tag.NotEmpty();
                });
        }
    }
}
