using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Authentication.Common;

namespace ProjectTemplate2024.Application.Authentication.Commands.GoogleLogin;

public record GoogleLoginCommand(string IdToken)
    : IRequest<ErrorOr<AuthenticationResult>>;
