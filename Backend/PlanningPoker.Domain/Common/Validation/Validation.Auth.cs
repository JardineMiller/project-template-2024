namespace PlanningPoker.Domain.Common.Validation;

public static partial class Validation
{
    public static class Auth
    {
        public static class Password
        {
            public const int MinLength = 6;
            public const int MaxLength = 50;
            public const string Pattern =
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";
        }
    }
}
