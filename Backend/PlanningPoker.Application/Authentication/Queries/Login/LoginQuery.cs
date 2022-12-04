using ErrorOr;
using MediatR;
using PlanningPoker.Application.Authentication.Common;

namespace PlanningPoker.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password)
    : IRequest<ErrorOr<AuthenticationResult>>;
