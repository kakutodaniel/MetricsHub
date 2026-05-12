using FluentValidation;

namespace MetricsHub.Application.Modules.Events.DTOs.Validators
{
    public class ResourceValidator : AbstractValidator<Resource>
    {
        public ResourceValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Group)
                .NotEmpty();

            RuleFor(x => x.Environment)
                .NotEmpty();
        }
    }
}
