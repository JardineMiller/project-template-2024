namespace PlanningPoker.Domain.Common.Validation;

public static partial class Validation
{
    public static class Game
    {
        public static class Name
        {
            public const int minLength = 3;
            public const int maxLength = 100;
        }

        public static class Description
        {
            public const int minLength = 3;
            public const int maxLength = 250;
        }

        public static class Code
        {
            public const int maxLength = 25;
        }
    }
}
