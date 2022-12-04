using Microsoft.AspNetCore.Identity;
using PlanningPoker.Domain.Entities.Interfaces;

namespace PlanningPoker.Domain.Entities;

public class User : IdentityUser, IAuditable
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset? ModifiedOn { get; set; }

    public virtual ICollection<Game> Games { get; set; } =
        new List<Game>();
}
