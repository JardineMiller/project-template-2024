using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Account.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Email,
    string NewPassword,
    string? Token = null,
    string? OldPassword = null
) : IRequest<ErrorOr<ResetPasswordResult>>;
