using PlanningPoker.Domain.Entities;

namespace PlanningPoker.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string? Token = null,
    string? RefreshToken = null,
    string? PlayerId = null
);
