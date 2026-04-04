namespace ProjectTemplate2024.Contracts.Account.GetUserDetails;

public record GetUserDetailsResponse(
    string Id,
    string DisplayName,
    string Email,
    string? Bio,
    string? AvatarUrl
);
