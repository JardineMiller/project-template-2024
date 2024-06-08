using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.Register;

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
