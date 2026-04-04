namespace ProjectTemplate2024.Contracts.Account.UpdateUser;

public record UpdateUserRequest(
    string Email,
    string DisplayName,
    string? Bio,
    string? AvatarUrl
);
