using Microsoft.AspNetCore.Identity;
using ProjectTemplate2024.Domain.Entities.Interfaces;

namespace ProjectTemplate2024.Domain.Entities;

public class User : IdentityUser, IAuditable
{
    public string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarFileName { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } =
        new List<RefreshToken>();
}
