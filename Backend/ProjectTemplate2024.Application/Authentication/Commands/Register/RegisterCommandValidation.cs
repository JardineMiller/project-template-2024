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
                Validation.User.FirstName.MinLength,
                Validation.User.FirstName.MaxLength
            );
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(
                Validation.User.LastName.MinLength,
                Validation.User.LastName.MaxLength
            );
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(
                Validation.Auth.Password.MinLength,
                Validation.Auth.Password.MaxLength
            )
            .Matches(Validation.Auth.Password.Pattern);
    }
}
