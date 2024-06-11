using Microsoft.Extensions.Options;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Application.Settings;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly ClientAppSettings _clientSettings;

    public BlobStorageService(IOptions<ClientAppSettings> clientSettings)
    {
        if (clientSettings == null)
        {
            throw new ArgumentNullException(nameof(clientSettings));
        }

        this._clientSettings = clientSettings.Value;
    }

    public string? GetAvatarUrl(User user, string? fileName)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (fileName == null)
        {
            return null;
        }

        return $"{this._clientSettings.BlobUrl}/{user.Id}/images/{fileName}";
    }
}
