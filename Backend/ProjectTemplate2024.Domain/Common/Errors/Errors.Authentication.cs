using ErrorOr;

namespace ProjectTemplate2024.Domain.Common.Errors;

public static partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials = Error.Validation(
            code: "Auth.InvalidCredentials",
            description: "Invalid credentials."
        );

        public static Error EmailNotConfirmed = Error.Validation(
            code: "Auth.EmailNotConfirmed",
            description: "Email not confirmed."
        );

        public static Error TokenExpired = Error.Validation(
            code: "Auth.ExpiredToken",
            description: "Unable to refresh token as it is expired"
        );
    }
}
