using MediatR;
using ErrorOr;
using PlanningPoker.Application.Account.Common;

namespace PlanningPoker.Application.Account.Commands.RequestResetPassword;

public record RequestResetPasswordCommand(string Email)
    : IRequest<ErrorOr<RequestResetPasswordResult>>;
