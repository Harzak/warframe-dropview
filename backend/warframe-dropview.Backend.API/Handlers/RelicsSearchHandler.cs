using warframe_dropview.Backend.Abstractions.Repositories;

namespace warframe_dropview.Backend.API.Handlers;

/// <summary>
/// Handles search queries for relics and returns the corresponding search results.
/// </summary>
internal sealed class RelicsSearchHandler : IRequestHandler<RelicsSearchQuery, OperationResult<SearchResultDto>>
{
    private readonly IMissionDropRepository _missionDropRepository;

    public RelicsSearchHandler(IMissionDropRepository missionDropRepository)
    {
        _missionDropRepository = missionDropRepository ?? throw new ArgumentNullException(nameof(missionDropRepository));
    }

    public async Task<OperationResult<SearchResultDto>> Handle(RelicsSearchQuery request, CancellationToken cancellationToken)
    {
        OperationResult<SearchResultDto> result = new();

        if (request is null)
        {
            return result.WithError("Request cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(request.ItemName))
        {
            return result.WithError("Item name cannot be null or whitespace.");
        }

        IEnumerable<MissionDrop> drops = await _missionDropRepository.SearchDropsAsync(
            request.ItemName,
            request.DropRarities,
            "relic",
            request.RelicTiers,
            request.MissionTypes,
            request.Offset,
            request.Limit).ConfigureAwait(false);

        SearchResultDto searchResult = new();

        foreach (MissionDrop drop in drops)
        {
            MissionDropDto dto = new()
            {
                Name = drop.Name,
                DropRate = drop.DropRate,
                Rarity = drop.Rarity,
                MissionName = drop.Mission.Name,
                MissionType = drop.Mission.Type,
                Planet = drop.Mission.Planet,
                Rotation = drop.Mission.Rotation,
                Type = drop.Type,
                Subtype = drop.Subtype
            };

            searchResult.MissionDrops.Add(dto);
        }

        return result.WithValue(searchResult).WithSuccess();
    }
}