using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProjectTemplate2024.Application.Account.Commands.UploadImage;

public class UploadImageCommand : IRequest<ErrorOr<UploadImageResult>>
{
    public IFormFile File { get; set; }

    public UploadImageCommand(IFormFile file)
    {
        this.File = file;
    }
}
