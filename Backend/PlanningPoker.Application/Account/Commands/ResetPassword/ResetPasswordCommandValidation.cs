using FluentValidation;
using PlanningPoker.Domain.Common.Validation;

namespace PlanningPoker.Application.Account.Commands.ResetPassword;

public class ResetPasswordCommandValidation
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Length(
                Validation.Auth.Password.MinLength,
                Validation.Auth.Password.MaxLength
            )
            .Matches(Validation.Auth.Password.Pattern);

        RuleFor(x => x.NewPassword)
            .NotEqual(x => x.OldPassword)
            .When(x => !string.IsNullOrEmpty(x.OldPassword));

        RuleFor(x => x.Token)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.OldPassword));

        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .Length(
                Validation.Auth.Password.MinLength,
                Validation.Auth.Password.MaxLength
            )
            .Matches(Validation.Auth.Password.Pattern)
            .When(x => string.IsNullOrEmpty(x.Token));

        When(
            x =>
                !string.IsNullOrEmpty(x.Token)
                && !string.IsNullOrEmpty(x.OldPassword),
            () =>
            {
                RuleFor(x => x.Token).Null();
                RuleFor(x => x.OldPassword).Null();
            }
        );
    }
}
