using System.Web;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Commands.RequestResetPassword;

public class RequestResetPasswordCommandHandler
    : IRequestHandler<
          RequestResetPasswordCommand,
          ErrorOr<RequestResetPasswordResult>
      >
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public RequestResetPasswordCommandHandler(
        UserManager<User> userManager,
        IEmailService emailService
    )
    {
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<ErrorOr<RequestResetPasswordResult>> Handle(
        RequestResetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userManager.FindByEmailAsync(
            request.Email
        );

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token =
            await _userManager.GeneratePasswordResetTokenAsync(
                user
            );

        var encodedToken = HttpUtility.UrlEncode(token);

        _emailService.SendPasswordResetEmail(user.Email, token);

        return new RequestResetPasswordResult(encodedToken);
    }
}
