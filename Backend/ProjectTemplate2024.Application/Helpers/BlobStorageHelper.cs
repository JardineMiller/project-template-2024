using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Helpers;

public static class BlobStorageHelper
{
    public static string? GetAvatarUrl(User user, string? fileName)
    {
        if (user is null)
        {
            throw new ArgumentException("User must be provided");
        }

        if (fileName is null)
        {
            return null;
        }

        return $"images/{user.Id}/{fileName}";
    }
}
