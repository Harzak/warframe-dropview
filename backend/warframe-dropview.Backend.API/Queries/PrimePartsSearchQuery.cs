namespace warframe_dropview.Backend.API.Queries;

/// <summary>
/// Represents a query for searching prime parts with optional filters for drop type, part type, relic tier, and drop rarity.
/// </summary>
internal sealed class PrimePartsSearchQuery : BaseSearchQuery, IRequest<OperationResult<SearchResultDto>>
{
    public string? RelicTiers { get; set; }
    public string? DropRarities { get; set; }
    public string? Refinements { get; set; }
}