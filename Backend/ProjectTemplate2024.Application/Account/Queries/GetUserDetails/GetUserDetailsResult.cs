using ProjectTemplate2024.Domain.Entities;

namespace ProjectTemplate2024.Application.Account.Queries.GetUserDetails;

public record GetUserDetailsResult(User User, string? AvatarUrl);
