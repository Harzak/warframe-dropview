namespace warframe_dropview.Backend.API.Queries;

/// <summary>
/// Represents a query for searching prime parts with optional filters for drop type, part type, relic tier, and drop rarity.
/// </summary>
internal sealed class PrimePartsSearchQuery : BaseSearchQuery, IRequest<OperationResult<SearchResultDto>>
{
    public string? DropType { get; set; }
    public string? PartType { get; set; }
    public string? RelicTier { get; set; }
    public string? DropRarity { get; set; }
}