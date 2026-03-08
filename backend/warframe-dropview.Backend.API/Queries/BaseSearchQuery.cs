namespace warframe_dropview.Backend.API.Queries;

/// <summary>
/// Represents the base class for search query parameters, including item name filtering and pagination settings.
/// </summary>
internal abstract class BaseSearchQuery
{
    public string? ItemName { get; set; }
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}