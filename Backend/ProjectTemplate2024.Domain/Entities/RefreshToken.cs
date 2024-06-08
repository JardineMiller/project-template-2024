using System.Text.Json.Serialization;

#pragma warning disable CS8618

namespace ProjectTemplate2024.Domain.Entities;

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

    public bool IsActive => this.RevokedOn is null && !this.IsExpired;

    public bool IsExpired => DateTimeOffset.UtcNow >= this.Expires;
}
