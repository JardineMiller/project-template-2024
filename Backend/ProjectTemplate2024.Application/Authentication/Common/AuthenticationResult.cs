using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string? Token = null,
    string? RefreshToken = null,
    string? AvatarUrl = null
);
