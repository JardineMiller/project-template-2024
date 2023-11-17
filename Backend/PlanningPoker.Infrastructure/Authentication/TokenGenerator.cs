using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlanningPoker.Application.Common.Interfaces.Authentication;
using PlanningPoker.Application.Common.Interfaces.Services;
using PlanningPoker.Domain.Entities;

#pragma warning disable SYSLIB0023

namespace PlanningPoker.Infrastructure.Authentication;

public class TokenGenerator : ITokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public TokenGenerator(
        IDateTimeProvider dateTimeProvider,
        IOptions<JwtSettings> jwtSettings
    )
    {
        this._dateTimeProvider = dateTimeProvider;
        this._jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwt(User user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(this._jwtSettings.Secret)
            ),
            SecurityAlgorithms.HmacSha256
        );
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName!),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            )
        };

        var securityToken = new JwtSecurityToken(
            issuer: this._jwtSettings.Issuer,
            audience: this._jwtSettings.Audience,
            expires: this._dateTimeProvider.UtcNow.AddMinutes(
                this._jwtSettings.ExpiryMinutes
            ),
            claims: claims,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(
            securityToken
        );
    }

    public RefreshToken GenerateRefreshToken()
    {
        using var rngCryptoServiceProvider =
            new RNGCryptoServiceProvider();

        var randomBytes = new byte[64];
        rngCryptoServiceProvider.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            Expires = this._dateTimeProvider.UtcNow.AddDays(7),
            CreatedOn = this._dateTimeProvider.UtcNow,
        };
    }
}
