using ErrorOr;

namespace PlanningPoker.Domain.Common.Errors;

public static partial class Errors
{
    public static class Common
    {
        public static Error NotFound(string entityType)
        {
            return Error.NotFound(
                code: $"{entityType}.NotFound",
                description: $"{entityType} not found"
            );
        }
    }
}
