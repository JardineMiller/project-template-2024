using ErrorOr;
using MediatR;
using PlanningPoker.Application.Authentication.Common;

namespace PlanningPoker.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;
