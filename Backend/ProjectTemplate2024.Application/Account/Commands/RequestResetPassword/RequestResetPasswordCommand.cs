using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;

public record RequestResetPasswordCommand(string Email)
    : IRequest<ErrorOr<RequestResetPasswordResult>>;
