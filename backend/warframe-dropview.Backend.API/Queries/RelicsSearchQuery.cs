namespace warframe_dropview.Backend.API.Queries;

internal sealed class RelicsSearchQuery : IRequest<OperationResult<SearchResultDto>>
{
    public string? RelicName { get; set; } 
}