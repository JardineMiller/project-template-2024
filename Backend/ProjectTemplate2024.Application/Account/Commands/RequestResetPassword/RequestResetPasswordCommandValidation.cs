using FluentValidation;

namespace ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;

public class RequestResetPasswordCommandValidation
    : AbstractValidator<RequestResetPasswordCommand>
{
    public RequestResetPasswordCommandValidation()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
