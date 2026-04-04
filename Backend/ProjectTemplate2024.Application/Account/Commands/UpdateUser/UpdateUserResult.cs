using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Commands.UpdateUser;

public record UpdateUserResult(User User, string? AvatarUrl);
