using ErrorOr;
using MediatR;
using ProjectTemplate2024.Application.Common.Interfaces.Services;
using ProjectTemplate2024.Domain.Common.Errors;
using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Commands.UploadImage;

public class UploadImageCommandHandler
    : IRequestHandler<UploadImageCommand, ErrorOr<UploadImageResult>>
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICurrentUserService _currentUserService;

    public UploadImageCommandHandler(
        IBlobStorageService blobStorageService,
        ICurrentUserService currentUserService
    )
    {
        this._blobStorageService = blobStorageService;
        this._currentUserService = currentUserService;
    }

    public async Task<ErrorOr<UploadImageResult>> Handle(
        UploadImageCommand request,
        CancellationToken cancellationToken
    )
    {
        // Validate file size
        // Validate file type
        var userId = this._currentUserService.UserId;

        if (userId is null)
        {
            return Errors.Common.NotFound(
                $"{nameof(User)} not found [{userId}]"
            );
        }

        var result = await this._blobStorageService.UploadFile(
            request.File,
            userId,
            cancellationToken
        );

        if (result is null)
        {
            return Errors.Common.NotFound(
                "Something went wrong uploading the file."
            );
        }

        return new UploadImageResult(result);
    }
}
