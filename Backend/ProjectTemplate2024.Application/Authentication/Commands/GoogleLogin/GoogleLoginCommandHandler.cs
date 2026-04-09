using ErrorOr;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Application.Settings;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.GoogleLogin;

public class GoogleLoginCommandHandler
    : IRequestHandler<GoogleLoginCommand, ErrorOr<AuthenticationResult>>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly GoogleSettings _googleSettings;

    public GoogleLoginCommandHandler(
        UserManager<User> userManager,
        IUserRepository userRepository,
        ITokenGenerator tokenGenerator,
        IOptions<GoogleSettings> googleSettings
    )
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _googleSettings = googleSettings.Value;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        GoogleLoginCommand cmd,
        CancellationToken cancellationToken
    )
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _googleSettings.ClientId },
            };

            payload = await GoogleJsonWebSignature.ValidateAsync(
                cmd.IdToken,
                settings
            );
        }
        catch (InvalidJwtException e)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var user = await _userRepository.GetUserByEmail(
            payload.Email,
            cancellationToken,
            u => u.RefreshTokens
        );

        if (user is null)
        {
            user = new User
            {
                DisplayName = payload.Name ?? payload.Email,
                Email = payload.Email,
                UserName = payload.Email,
                EmailConfirmed = true,
                AvatarFileName = payload.Picture,
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                return Errors.User.CreationFailed;
            }
        }
        else
        {
            // Update avatar from Google profile if not already set
            if (
                string.IsNullOrWhiteSpace(user.AvatarFileName)
                && !string.IsNullOrWhiteSpace(payload.Picture)
            )
            {
                user.AvatarFileName = payload.Picture;
                await _userRepository.UpdateUser(user, cancellationToken);
            }
        }

        // Generate tokens
        var jwt = _tokenGenerator.GenerateJwt(user);

        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userRepository.UpdateUser(user, cancellationToken);

        return new AuthenticationResult(
            user,
            jwt,
            newRefreshToken.Token,
            user.AvatarFileName
        );
    }
}
