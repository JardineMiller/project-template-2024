using ErrorOr;
using MediatR;

namespace ProjectTemplate2024.Application.Account.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<ErrorOr<UpdateUserResult>>
{
    public string Email { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
}
