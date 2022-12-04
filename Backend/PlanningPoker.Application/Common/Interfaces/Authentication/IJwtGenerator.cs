using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Common.Interfaces.Authentication;

public interface IJwtGenerator
{
    string GenerateToken(User user);
}
