namespace warframe_dropview.Backend.API.Handlers;

/// <summary>
/// Handles search queries for mods and returns the corresponding search results.
/// </summary>
internal sealed class ModsSearchHandler : IRequestHandler<ModsSearchQuery, OperationResult<SearchResultDto>>
{
    private readonly IMissionDropRepository _missionDropsRepository;
    private readonly IEnemyDropRepository _enemyDropRepository;

    public ModsSearchHandler(IMissionDropRepository missionDropsRepository, IEnemyDropRepository enemyDropRepository)
    {
        _missionDropsRepository = missionDropsRepository;
        _enemyDropRepository = enemyDropRepository;
    }

    public async Task<OperationResult<SearchResultDto>> Handle(ModsSearchQuery request, CancellationToken cancellationToken)
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

        IEnumerable<MissionDrop> missionDrops = await _missionDropsRepository.SearchDropsAsync(
            request.ItemName,
            request.DropRarities,
            request.ItemTypes,
            request.Subtypes,
            request.MissionTypes,
            request.Offset,
            request.Limit).ConfigureAwait(false);

        SearchResultDto searchResult = new();

        foreach (MissionDrop missionDrop in missionDrops)
        {
            MissionDropDto dto = new()
            {
                Name = missionDrop.Name,
                DropRate = missionDrop.DropRate,
                MissionName = missionDrop.Mission.Name,
                MissionType = missionDrop.Mission.Type,
                Planet = missionDrop.Mission.Planet,
                Rotation = missionDrop.Mission.Rotation,
                Rarity = missionDrop.Rarity,
                Type = missionDrop.Type
            };

            searchResult.MissionDrops.Add(dto);
        }

        IEnumerable<EnemyDrop> enemyDrops = await _enemyDropRepository.SearchDropsAsync(
            request.ItemName,
            request.DropRarities,
            request.ItemTypes,
            request.Offset,
            request.Limit).ConfigureAwait(false);

        foreach (EnemyDrop enemyDrop in enemyDrops)
        {
            EnemyDropDto dto = new()
            {
                Name = enemyDrop.Name,
                DropRate = enemyDrop.DropRate,
                EnemyName = enemyDrop.Enemy.Name,
                Rarity = enemyDrop.Rarity,
                Type = enemyDrop.Type
            };

            searchResult.EnemyDrops.Add(dto);
        }

        return result.WithValue(searchResult).WithSuccess();
    }
}