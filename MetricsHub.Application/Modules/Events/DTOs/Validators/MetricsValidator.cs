using FluentValidation;

namespace MetricsHub.Application.Modules.Events.DTOs.Validators
{
    public class MetricsValidator : AbstractValidator<Metrics>
    {
        public MetricsValidator()
        {
            RuleFor(x => x.CpuPct)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.MemMb)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ActiveConns)
                .GreaterThanOrEqualTo(0);
        }
    }
}
