using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Common.Interfaces.Authentication;

public interface ITokenGenerator
{
    string GenerateJwt(User user);
    RefreshToken GenerateRefreshToken();
}
