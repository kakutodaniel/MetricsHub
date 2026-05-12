using FluentValidation;

namespace MetricsHub.Application.Modules.Events.DTOs.Validators
{
    public class RuleValidator : AbstractValidator<Rule>
    {
        public RuleValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Threshold)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Actual)
                .NotNull();

            RuleFor(x => x.Metric)
                .NotEmpty();

            RuleFor(x => x.Severity)
                .NotEmpty();
        }
    }
}
