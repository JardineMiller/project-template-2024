namespace ProjectTemplate2024.Contracts.Account.ResetPassword;

public record ResetPasswordRequest(
    string Email,
    string NewPassword,
    string? Token = null,
    string? OldPassword = null
);
