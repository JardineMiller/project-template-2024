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
        this._userManager = userManager;
        this._emailService = emailService;
    }

    public async Task<ErrorOr<RequestResetPasswordResult>> Handle(
        RequestResetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await this._userManager.FindByEmailAsync(
            request.Email
        );

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token =
            await this._userManager.GeneratePasswordResetTokenAsync(
                user
            );

        var encodedToken = HttpUtility.UrlEncode(token);

        this._emailService.SendPasswordResetEmail(user.Email, token);

        return new RequestResetPasswordResult(encodedToken);
    }
}
