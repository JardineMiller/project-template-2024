using MediatR;
using ErrorOr;
using ProjectTemplate2024.Application.Authentication.Common;

namespace ProjectTemplate2024.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(string Email, string Token)
    : IRequest<ErrorOr<AuthenticationResult>>;
