namespace PlanningPoker.Domain.Common.Validation;

public static class ValidationConstants
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

    public static class User
    {
        public static class FirstName
        {
            public const int MinLength = 2;
            public const int MaxLength = 50;
        }

        public static class LastName
        {
            public const int MinLength = 2;
            public const int MaxLength = 50;
        }
    }

    public static class Game
    {
        public static class Name
        {
            public const int MinLength = 3;
            public const int MaxLength = 100;
        }

        public static class Description
        {
            public const int MinLength = 3;
            public const int MaxLength = 250;
        }

        public static class Code
        {
            public const int MaxLength = 25;
        }
    }
}
