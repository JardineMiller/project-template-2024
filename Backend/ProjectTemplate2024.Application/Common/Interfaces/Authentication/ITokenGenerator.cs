using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Common.Interfaces.Authentication;

public interface ITokenGenerator
{
    string GenerateJwt(User user);
    RefreshToken GenerateRefreshToken();
}
