using FluentValidation;
using ProjectTemplate2024.Domain.Common.Validation;

namespace ProjectTemplate2024.Application.Authentication.Commands.Register;

public class RegisterCommandValidation : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .Length(
                Validation.User.DisplayName.MinLength,
                Validation.User.DisplayName.MaxLength
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
