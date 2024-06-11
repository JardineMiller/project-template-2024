using Microsoft.AspNetCore.Http;

namespace ProjectTemplate2024.Application.Common.Interfaces.Services;

public interface IBlobStorageService
{
    string? GetAvatarUrl(string userId, string? fileName);

    Task<string?> UploadFile(
        IFormFile file,
        string userId,
        CancellationToken cancellationToken
    );
}
