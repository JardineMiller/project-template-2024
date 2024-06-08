using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Authentication.Common;

namespace ProjectTemplate2024.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;
