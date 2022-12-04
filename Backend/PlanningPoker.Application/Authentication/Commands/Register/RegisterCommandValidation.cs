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
                ValidationConstants.User.FirstName.MinLength,
                ValidationConstants.User.FirstName.MaxLength
            );
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(
                ValidationConstants.User.LastName.MinLength,
                ValidationConstants.User.LastName.MaxLength
            );
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(
                ValidationConstants.Auth.Password.MinLength,
                ValidationConstants.Auth.Password.MaxLength
            )
            .Matches(ValidationConstants.Auth.Password.Pattern);
    }
}
