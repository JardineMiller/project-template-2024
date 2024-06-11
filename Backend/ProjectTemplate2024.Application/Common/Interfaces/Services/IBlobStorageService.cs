using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Common.Interfaces.Services;

public interface IBlobStorageService
{
    string? GetAvatarUrl(User user, string? fileName);
}
