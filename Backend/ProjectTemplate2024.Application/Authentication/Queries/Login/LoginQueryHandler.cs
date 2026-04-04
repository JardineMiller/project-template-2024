using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Authentication.Common;
using ProjectTemplate2024.Application.Common.Interfaces.Authentication;
using ProjectTemplate2024.Application.Common.Interfaces.Repositories;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Queries.Login;

public class LoginQueryHandler
    : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBlobStorageService _blobStorageService;

    public LoginQueryHandler(
        ITokenGenerator tokenGenerator,
        IDateTimeProvider dateTimeProvider,
        IUserRepository userRepository,
        IBlobStorageService blobStorageService
    )
    {
        _tokenGenerator = tokenGenerator;
        _dateTimeProvider = dateTimeProvider;
        _userRepository = userRepository;
        _blobStorageService = blobStorageService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(
        LoginQuery qry,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetUserByEmail(
            qry.Email,
            cancellationToken,
            x => x.RefreshTokens
        );

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        if (!user.EmailConfirmed)
        {
            return Errors.Authentication.EmailNotConfirmed;
        }

        if (!await _userRepository.CheckPasswordAsync(user, qry.Password, cancellationToken))
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var oldRefreshToken = user.RefreshTokens
            .Where(x => x.IsActive)
            .MaxBy(x => x.CreatedOn);

        // replace old refresh token with a new one and save
        var newRefreshToken = _tokenGenerator.GenerateRefreshToken();

        if (oldRefreshToken is not null)
        {
            oldRefreshToken.RevokedOn = _dateTimeProvider.UtcNow;
            oldRefreshToken.ReplacedBy = newRefreshToken.Token;
        }

        user.RefreshTokens.Add(newRefreshToken);
        await _userRepository.UpdateUser(user, cancellationToken);

        // generate new jwt
        var jwt = _tokenGenerator.GenerateJwt(user);

        var response = new AuthenticationResult(
            user,
            jwt,
            newRefreshToken.Token,
            _blobStorageService.GetAvatarUrl(user.Id, user.AvatarFileName)
        );

        return response;
    }
}
