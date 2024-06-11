namespace ProjectTemplate2024.Contracts.Authentication;

public record AuthenticationResponse(
    Guid Id,
    string DisplayName,
    string Email,
    string? Token = null,
    string? AvatarUrl = null
);
