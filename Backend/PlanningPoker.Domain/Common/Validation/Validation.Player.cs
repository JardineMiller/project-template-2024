namespace PlanningPoker.Domain.Common.Validation;

public static partial class Validation
{
    public static class Player
    {
        public static class DisplayName
        {
            public const int MinLength = 2;
            public const int MaxLength = 100;
        }
    }
}
