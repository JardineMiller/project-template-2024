using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Authentication.Common;

namespace ProjectTemplate2024.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password)
    : IRequest<ErrorOr<AuthenticationResult>>;
