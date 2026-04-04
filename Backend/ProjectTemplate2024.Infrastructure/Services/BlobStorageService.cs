using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Infrastructure.Persistence;

namespace ProjectTemplate2024.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly DatabaseSettings _databaseSettings;

    public BlobStorageService(IOptions<DatabaseSettings> databaseSettings)
    {
        if (databaseSettings == null)
        {
            throw new ArgumentNullException(nameof(databaseSettings));
        }

        _databaseSettings = databaseSettings.Value;
    }

    public string? GetAvatarUrl(string userId, string? fileName)
    {
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        if (fileName == null)
        {
            return null;
        }

        return $"{_databaseSettings.BlobUrl}/{userId}/images/{fileName}";
    }

    public async Task<string?> UploadFile(
        IFormFile file,
        string userId,
        CancellationToken cancellationToken = new()
    )
    {
        var client = new BlobServiceClient(
            _databaseSettings.BlobConnectionString
        );
        var containerClient = client.GetBlobContainerClient(userId);

        if (!await containerClient.ExistsAsync(cancellationToken))
        {
            await containerClient.CreateAsync(
                publicAccessType: PublicAccessType.Blob,
                cancellationToken: cancellationToken
            );
        }

        var blobClient = containerClient.GetBlobClient(
            $"images/{file.FileName}"
        );

        using (var stream = file.OpenReadStream())
        {
            var upload = await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new() { ContentType = file.ContentType }
                },
                cancellationToken
            );

            if (upload.HasValue)
            {
                return GetAvatarUrl(userId, file.FileName);
            }
        }

        return null;
    }

    public async Task<bool> DeleteFile(
        string userId,
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        var client = new BlobServiceClient(
            _databaseSettings.BlobConnectionString
        );

        var containerClient = client.GetBlobContainerClient(userId);

        if (!await containerClient.ExistsAsync(cancellationToken))
        {
            return false;
        }

        var blobClient = containerClient.GetBlobClient($"images/{fileName}");

        var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return response.Value;
    }
}
