using FluentValidation;

namespace PlanningPoker.Application.Common.Validators;

public static class ValidatorExtensions
{
    public static void IsGuid<T>(
        this IRuleBuilder<T, string> ruleBuilder
    )
    {
        ruleBuilder
            .Must(s => Guid.TryParse(s, out _))
            .WithMessage("'{PropertyName}' must be a valid GUID.");
    }
}
