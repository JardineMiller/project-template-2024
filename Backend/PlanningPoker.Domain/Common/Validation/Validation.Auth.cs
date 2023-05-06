namespace PlanningPoker.Domain.Common.Validation;

public static partial class Validation
{
    public static class Auth
    {
        public static class Password
        {
            public const int minLength = 6;
            public const int maxLength = 50;
            public const string pattern =
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";
        }
    }
}
