namespace warframe_dropview.Backend.API.Queries;

internal sealed class ModsSearchQuery : BaseSearchQuery, IRequest<OperationResult<SearchResultDto>>
{
    public string? DropRarities { get; set; }
    public string? ItemTypes { get; set; }
    public string? Subtypes { get; set; }
    public string? MissionTypes { get; set; }
}