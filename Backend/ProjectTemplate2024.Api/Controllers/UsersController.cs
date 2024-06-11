using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate2024.Application.Account.Commands.UploadImage;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.UploadImage;

namespace ProjectTemplate2024.Api.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet(nameof(Details))]
    public async Task<IActionResult> Details()
    {
        var cmd = new GetUserDetailsQuery();
        var userDetailsResult = await this._mediator.Send(cmd);

        return userDetailsResult.Match(
            success => Ok(success.Adapt<GetUserDetailsResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpPost(nameof(Upload))]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var cmd = new UploadImageCommand(file);
        var result = await this._mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<UploadImageResponse>()),
            errors => Problem(errors)
        );
    }
}
