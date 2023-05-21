using System.Text.Json.Serialization;
using PlanningPoker.Domain.Entities;

public class RefreshToken
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public virtual User User { get; set; }

    public string Token { get; set; }

    public DateTimeOffset Expires { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset? RevokedOn { get; set; }

    public string? ReplacedBy { get; set; }

    public bool IsActive => this.RevokedOn == null && !this.IsExpired;

    public bool IsExpired => DateTimeOffset.UtcNow >= this.Expires;
}
