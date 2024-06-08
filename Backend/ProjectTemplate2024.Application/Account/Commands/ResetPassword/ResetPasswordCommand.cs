using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Account.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string NewPassword,
    string? Token = null,
    string? OldPassword = null
) : IRequest<ErrorOr<ResetPasswordResult>>;
