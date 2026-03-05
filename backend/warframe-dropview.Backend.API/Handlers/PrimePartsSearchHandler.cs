namespace warframe_dropview.Backend.API.Handlers;

/// <summary>
/// Handles search queries for prime part relic drops and returns the corresponding search results.
/// </summary>
internal sealed class PrimePartsSearchHandler : IRequestHandler<PrimePartsSearchQuery, OperationResult<SearchResultDto>>
{
    private readonly IRelicDropRepository _relicDropRepository;

    public PrimePartsSearchHandler(IRelicDropRepository relicDropRepository)
    {
        _relicDropRepository = relicDropRepository ?? throw new ArgumentNullException(nameof(relicDropRepository));
    }

    public async Task<OperationResult<SearchResultDto>> Handle(PrimePartsSearchQuery request, CancellationToken cancellationToken)
    {
        OperationResult<SearchResultDto> result = new();

        if (request is null)
        {
            return result.WithError("Request cannot be null.");
        }

        IEnumerable<RelicDrop> drops = await _relicDropRepository.SearchDropsAsync(
           request.DropType,
           request.PartType,
           request.RelicTier,
           request.DropRarity).ConfigureAwait(false);

        SearchResultDto searchResult = new();

        foreach (RelicDrop drop in drops)
        {
            RelicDropDto dto = new()
            {
                Name = drop.Name,
                DropRate = drop.DropRate,
                Rarity = drop.Rarity,
                PartType = drop.Subtype,
                RelicTier = drop.Relic.Tier,
                RelicCode = drop.Relic.Code
            };

            searchResult.RelicDrops.Add(dto);
        }

        return result.WithValue(searchResult);
    }
}