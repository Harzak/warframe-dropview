namespace warframe_dropview.Backend.API.Queries;

internal sealed class RelicsSearchQuery : BaseSearchQuery, IRequest<OperationResult<SearchResultDto>>
{
    public string? RelicTiers { get; set; }
    public string? DropRarities { get; set; }
    public string? MissionTypes { get; set; }
}