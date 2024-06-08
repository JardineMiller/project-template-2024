namespace ProjectTemplate2024.Domain.Common.Validation;

public static partial class Validation
{
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
}
