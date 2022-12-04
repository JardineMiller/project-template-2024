using ErrorOr;
using MediatR;

namespace PlanningPoker.Application.Account.Commands.RequestResetPassword;

public record RequestResetPasswordCommand(string Email)
    : IRequest<ErrorOr<RequestResetPasswordResult>>;
