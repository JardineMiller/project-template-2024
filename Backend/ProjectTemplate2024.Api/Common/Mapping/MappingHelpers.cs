namespace ProjectTemplate2024.Api.Common.Mapping;

public static class MappingHelpers
{
    public static string TrimToEmpty(this string? s) =>
        s?.Trim() ?? string.Empty;

    public static string? TrimToNull(this string? s) =>
        string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
