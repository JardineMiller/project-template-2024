using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate2024.Application.Account.Commands.DeleteAvatar;
using ProjectTemplate2024.Application.Account.Commands.UpdateUser;
using ProjectTemplate2024.Application.Account.Commands.UploadUserAvatar;
using ProjectTemplate2024.Application.Account.Queries.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.GetUserDetails;
using ProjectTemplate2024.Contracts.Account.UpdateUser;
using ProjectTemplate2024.Contracts.Account.UploadUserAvatar;

namespace ProjectTemplate2024.Api.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(nameof(Details))]
    public async Task<IActionResult> Details()
    {
        var cmd = new GetUserDetailsQuery();
        var userDetailsResult = await _mediator.Send(cmd);

        return userDetailsResult.Match(
            success => Ok(success.Adapt<GetUserDetailsResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpPost(nameof(UploadAvatar))]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var cmd = new UploadUserAvatarCommand(file);
        var result = await _mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<UploadUserAvatarResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserRequest request)
    {
        var cmd = request.Adapt<UpdateUserCommand>();
        var result = await _mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<UpdateUserResponse>()),
            errors => Problem(errors)
        );
    }

    [HttpDelete(nameof(DeleteAvatar))]
    public async Task<IActionResult> DeleteAvatar([FromQuery] string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Problem("fileName is required", statusCode: 400);
        }

        var cmd = new DeleteAvatarCommand(fileName);
        var result = await _mediator.Send(cmd);

        return result.Match(
            success => Ok(success.Adapt<UpdateUserResponse>()),
            errors => Problem(errors)
        );
    }
}
