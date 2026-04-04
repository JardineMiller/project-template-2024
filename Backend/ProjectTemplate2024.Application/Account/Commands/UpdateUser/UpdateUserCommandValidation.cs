using FluentValidation;
using ProjectTemplate2024.Domain.Common.Validation;
using System;

namespace ProjectTemplate2024.Application.Account.Commands.UpdateUser;

public class UpdateUserCommandValidation : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .Length(
                Validation.User.DisplayName.MinLength,
                Validation.User.DisplayName.MaxLength
            );
        RuleFor(x => x.Bio).MaximumLength(Validation.User.Bio.MaxLength);
        RuleFor(x => x.AvatarUrl).Must(x => string.IsNullOrEmpty(x) || Uri.IsWellFormedUriString(x, UriKind.Absolute));
    }
}
