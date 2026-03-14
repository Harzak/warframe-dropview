namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class MissionDropRepository : IMissionDropRepository
{
    private readonly IMongoCollection<MissionDrop> _db;

    public MissionDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<MissionDrop>("mission_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<MissionDrop>> SearchDropsAsync(string itemName, 
        string? dropRarities,
        string? itemTypes,
        string? itemSubTypes,
        string? missionTypes,
        int? offset, 
        int? limit)
    {
        FilterDefinitionBuilder<MissionDrop> builder = Builders<MissionDrop>.Filter;
        FilterDefinition<MissionDrop> filter = builder.Empty;
        
        if (!string.IsNullOrWhiteSpace(itemName))
        {
            filter &= builder.Regex(d => d.Name, new BsonRegularExpression(itemName, "i"));
        }

        if (!string.IsNullOrWhiteSpace(dropRarities))
        {
            string[] rarities = dropRarities.Split(',').Select(r => r.Trim()).ToArray();
            filter &= builder.In(d => d.Rarity, rarities);
        }

        if (!string.IsNullOrWhiteSpace(itemTypes))
        {
            string[] types = itemTypes.Split(',').Select(t => t.Trim()).ToArray();
            filter &= builder.In(d => d.Type, types);
        }

        if (!string.IsNullOrWhiteSpace(itemSubTypes))
        {
            string[] subTypes = itemSubTypes.Split(',').Select(st => st.Trim()).ToArray();
            filter &= builder.In(d => d.Subtype, subTypes);
        }

        if (!string.IsNullOrWhiteSpace(missionTypes))
        {
            string[] mTypes = missionTypes.Split(',').Select(mt => mt.Trim()).ToArray();
            filter &= builder.In(d => d.Mission.Type, mTypes);
        }

        List<MissionDrop> results = await _db.Find(filter)
            .Skip(offset ?? 0)
            .Limit(limit ?? 0)
            .ToListAsync().ConfigureAwait(false);

        return results;
    }

    public async Task InsertDropsAsync(IEnumerable<MissionDrop> drops)
    {
        if (!drops.Any())
        {
            return;
        }

        await _db.InsertManyAsync(drops).ConfigureAwait(false);
    }
}

