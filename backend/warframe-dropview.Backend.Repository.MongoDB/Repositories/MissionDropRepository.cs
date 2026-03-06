namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class MissionDropRepository : IMissionDropRepository
{
    private readonly IMongoCollection<MissionDrop> _db;

    public MissionDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<MissionDrop>("mission_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<MissionDrop>> SearchDropsAsync(string? itemName)
    {
        FilterDefinitionBuilder<MissionDrop> builder = Builders<MissionDrop>.Filter;
        FilterDefinition<MissionDrop> filter = builder.Empty;
        
        if (!string.IsNullOrWhiteSpace(itemName))
        {
            filter &= builder.Eq(d => d.Name, itemName);
        }

        IAsyncCursor<MissionDrop> cursor = await _db.FindAsync(filter).ConfigureAwait(false);
        List<MissionDrop> results = await cursor.ToListAsync().ConfigureAwait(false);

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

