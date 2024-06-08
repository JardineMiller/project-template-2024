using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PlanningPoker.Application.Authentication.Common;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Domain.Common.Errors;
using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Authentication.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;

    public RegisterCommandHandler(
        UserManager<User> userManager,
        IEmailService emailService
    )
    {
        this._userManager = userManager;
        this._emailService = emailService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        RegisterCommand cmd,
        CancellationToken cancellationToken
    )
    {
        // Check if user already exists
        if (
            await this._userManager.FindByEmailAsync(cmd.Email)
            is not null
        )
        {
            return Errors.User.DuplicateEmail;
        }

        // Create user (generate unique id)
        var user = new User
        {
            FirstName = cmd.FirstName,
            LastName = cmd.LastName,
            Email = cmd.Email,
            UserName = cmd.Email,
        };

        var result = await this._userManager.CreateAsync(
            user,
            cmd.Password
        );

        if (!result.Succeeded)
        {
            return Errors.User.CreationFailed;
        }

        this._emailService.SendConfirmationEmail(
            user.Email,
            user.FirstName,
            await this._userManager.GenerateEmailConfirmationTokenAsync(
                user
            )
        );

        return new AuthenticationResult(user);
    }
}
