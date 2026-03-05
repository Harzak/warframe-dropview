namespace warframe_dropview.Backend.API.Handlers;

/// <summary>
/// Handles search queries for mods and returns the corresponding search results.
/// </summary>
internal sealed class ModsSearchHandler : IRequestHandler<ModsSearchQuery, OperationResult<SearchResultDto>>
{
    public Task<OperationResult<SearchResultDto>> Handle(ModsSearchQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}