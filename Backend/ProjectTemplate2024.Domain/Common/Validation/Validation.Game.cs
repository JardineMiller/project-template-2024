namespace PlanningPoker.Domain.Common.Validation;

public static partial class Validation
{
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
