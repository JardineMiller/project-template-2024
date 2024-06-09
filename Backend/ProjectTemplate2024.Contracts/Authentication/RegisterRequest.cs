namespace ProjectTemplate2024.Contracts.Authentication;

public record RegisterRequest(
    string DisplayName,
    string Email,
    string Password
);
