using FluentValidation;

namespace MetricsHub.Application.Modules.Events.DTOs.Validators
{
    public class AlertPayloadValidator : AbstractValidator<AlertPayload>
    {
        public AlertPayloadValidator()
        {
            RuleFor(x => x.AlertId)
                .NotEmpty();

            RuleFor(x => x.FiredAt)
                .BeValidDateTime()
                .WithMessage("Timestamp '{PropertyValue}' is not valid");

            RuleFor(x => x.Resource)
                .NotNull()
                .SetValidator(new ResourceValidator());

            RuleFor(x => x.Rule)
                .NotNull()
                .SetValidator(new RuleValidator());
        }
    }
}
