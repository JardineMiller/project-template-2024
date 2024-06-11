using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler
    : IRequestHandler<ConfirmEmailCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IBlobStorageService _blobStorageService;

    public ConfirmEmailCommandHandler(
        UserManager<User> userManager,
        ITokenGenerator tokenGenerator,
        IUserRepository userRepository,
        IBlobStorageService blobStorageService
    )
    {
        this._userManager = userManager;
        this._tokenGenerator = tokenGenerator;
        this._userRepository = userRepository;
        this._blobStorageService = blobStorageService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        ConfirmEmailCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var user = await this._userRepository.GetUserByEmail(
            cmd.Email,
            cancellationToken,
            u => u.RefreshTokens
        );

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var result = await this._userManager.ConfirmEmailAsync(user, cmd.Token);

        if (!result.Succeeded)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var token = this._tokenGenerator.GenerateJwt(user);

        // replace old refresh token with a new one and save
        var newRefreshToken = this._tokenGenerator.GenerateRefreshToken();

        user.RefreshTokens.Add(newRefreshToken);
        await this._userManager.UpdateAsync(user);

        return new AuthenticationResult(
            user,
            token,
            newRefreshToken.Token,
            this._blobStorageService.GetAvatarUrl(user.Id, user.AvatarFileName)
        );
    }
}
