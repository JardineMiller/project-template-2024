using MediatR;

namespace ProjectTemplate2024.Api.Controllers;

public class UsersController : ApiController
{
    private readonly ISender _mediator;

    public UsersController(ISender mediator)
    {
        this._mediator = mediator;
    }
}
