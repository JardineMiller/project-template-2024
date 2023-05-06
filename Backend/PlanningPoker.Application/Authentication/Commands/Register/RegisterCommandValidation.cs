using FluentValidation;
using PlanningPoker.Domain.Common.Validation;

namespace PlanningPoker.Application.Authentication.Commands.Register;

public class RegisterCommandValidation
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(
                Validation.User.FirstName.minLength,
                Validation.User.FirstName.maxLength
            );
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(
                Validation.User.LastName.minLength,
                Validation.User.LastName.maxLength
            );
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(
                Validation.Auth.Password.minLength,
                Validation.Auth.Password.maxLength
            )
            .Matches(Validation.Auth.Password.pattern);
    }
}
