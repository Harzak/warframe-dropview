namespace warframe_dropview.Backend.API.Handlers;

/// <summary>
/// Handles search queries for relics and returns the corresponding search results.
/// </summary>
internal sealed class RelicsSearchHandler : IRequestHandler<RelicsSearchQuery, OperationResult<SearchResultDto>>
{
    public Task<OperationResult<SearchResultDto>> Handle(RelicsSearchQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}