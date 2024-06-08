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

        public static Error Duplicate(string entityType)
        {
            return Error.Conflict(
                code: $"{entityType}.Conflict",
                description: $"{entityType} already exists"
            );
        }
    }
}
