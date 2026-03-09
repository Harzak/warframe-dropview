namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class RelicDropRepository : IRelicDropRepository
{
    private readonly IMongoCollection<RelicDrop> _db;

    public RelicDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<RelicDrop>("relic_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<RelicDrop>> SearchDropsAsync(
         string itemName,
         string? dropType,
         string? partType,
         string? relicTier,
         string? dropRarity,
         int? offset,
         int? limit)
    {
        FilterDefinitionBuilder<RelicDrop> builder = Builders<RelicDrop>.Filter;
        FilterDefinition<RelicDrop> filter = builder.Empty;

        if (!string.IsNullOrEmpty(itemName))
        {
            filter &= builder.Regex(d => d.Name, new BsonRegularExpression(itemName, "i"));
        }

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
            IEnumerable<string> rarities = dropRarity.Split(',').Select(r => r.Trim().ToLowerInvariant());
            filter &= builder.In(d => d.Rarity, rarities);
        }

        List<RelicDrop> results = await _db.Find(filter)
            .Skip(offset ?? 0)
            .Limit(limit ?? 0)
            .ToListAsync().ConfigureAwait(false);

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