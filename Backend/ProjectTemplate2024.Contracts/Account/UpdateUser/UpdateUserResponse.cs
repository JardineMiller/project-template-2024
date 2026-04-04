namespace ProjectTemplate2024.Contracts.Account.UpdateUser;

public record UpdateUserResponse(
    string Id,
    string DisplayName,
    string Email,
    string? Bio,
    string? AvatarUrl
);
