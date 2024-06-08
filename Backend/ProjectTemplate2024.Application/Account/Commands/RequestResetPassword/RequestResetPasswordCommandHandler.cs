﻿using System.Web;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Account.Commands.RequestResetPassword;

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

        if (user == null)
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
