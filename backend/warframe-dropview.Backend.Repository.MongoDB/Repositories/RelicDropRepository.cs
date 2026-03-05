namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class RelicDropRepository : IRelicDropRepository
{
    private readonly IMongoCollection<RelicDrop> _db;

    public RelicDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<RelicDrop>("relic_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<RelicDrop>> SearchDropsAsync(
         string? dropType,
         string? partType,
         string? relicTier,
         string? dropRarity)
    {
        FilterDefinitionBuilder<RelicDrop> builder = Builders<RelicDrop>.Filter;
        FilterDefinition<RelicDrop> filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(dropType))
        {
            filter &= builder.Eq(d => d.Type, dropType);
        }

        if (!string.IsNullOrWhiteSpace(partType))
        {
            filter &= builder.Eq(d => d.Subtype, partType);
        }

        if (!string.IsNullOrWhiteSpace(relicTier))
        {
            filter &= builder.Eq(d => d.Relic.Tier, relicTier);
        }

        if (!string.IsNullOrWhiteSpace(dropRarity))
        {
            filter &= builder.Eq(d => d.Rarity, dropRarity);
        }

        IAsyncCursor<RelicDrop> cursor = await _db.FindAsync(filter).ConfigureAwait(false);
        List<RelicDrop> results = await cursor.ToListAsync().ConfigureAwait(false);

        return results;
    }

    public async Task InsertDropsAsync(IEnumerable<RelicDrop> drops)
    {
        if (!drops.Any())
        {
            return;
        }

        await _db.InsertManyAsync(drops).ConfigureAwait(false);
    }
}