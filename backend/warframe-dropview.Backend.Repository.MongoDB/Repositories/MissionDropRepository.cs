namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class MissionDropRepository : IMissionDropRepository
{
    private readonly IMongoCollection<MissionDrop> _db;

    public MissionDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<MissionDrop>("mission_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<MissionDrop>> SearchDropsAsync(string? itemName, int? offset, int? limit)
    {
        FilterDefinitionBuilder<MissionDrop> builder = Builders<MissionDrop>.Filter;
        FilterDefinition<MissionDrop> filter = builder.Empty;
        
        if (!string.IsNullOrWhiteSpace(itemName))
        {
            filter &= builder.Regex(d => d.Name, new BsonRegularExpression(itemName, "i"));
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

