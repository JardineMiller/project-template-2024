namespace ProjectTemplate2024.Infrastructure.Persistence;

public class DatabaseSettings
{
    public static readonly string SectionName = "Database";
    public string? ConnectionString { get; set; }
    public string? BlobUrl { get; set; }
    public string? BlobConnectionString { get; set; }
}
