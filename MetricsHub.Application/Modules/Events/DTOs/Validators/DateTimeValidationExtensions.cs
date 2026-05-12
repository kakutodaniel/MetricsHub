using FluentValidation;

namespace MetricsHub.Application.Modules.Events.DTOs.Validators
{
    public static class DateTimeValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> BeValidDateTime<T>(
            this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .Must(BeValid);
        }

        private static bool BeValid(string ts)
        {
            return DateTime.TryParse(ts, out var dt) &&
                 dt != DateTime.MinValue &&
                 dt != DateTime.MaxValue;
        }
    }
}
