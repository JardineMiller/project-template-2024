using MediatR;
using ErrorOr;
using PlanningPoker.Application.Authentication.Common;

namespace PlanningPoker.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string Email, string Token)
    : IRequest<ErrorOr<AuthenticationResult>>;
