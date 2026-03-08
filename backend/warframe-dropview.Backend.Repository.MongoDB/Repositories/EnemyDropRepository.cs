namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class EnemyDropRepository : IEnemyDropRepository
{
    private readonly IMongoCollection<EnemyDrop> _db;

    public EnemyDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<EnemyDrop>("enemy_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<EnemyDrop>> SearchDrops(string itemName, int? offset, int? limit)
    {
        FilterDefinitionBuilder<EnemyDrop> builder = Builders<EnemyDrop>.Filter;
        FilterDefinition<EnemyDrop  > filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(itemName))
        {
            filter &= builder.Eq(d => d.Name, itemName);
        }

        List<EnemyDrop> results = await _db.Find(filter)
            .Skip(offset ?? 0)
            .Limit(limit ?? 0)
            .ToListAsync().ConfigureAwait(false);

        return results;
    }

    public async Task InsertDropsAsync(IEnumerable<EnemyDrop> drops)
    {
        if (!drops.Any())
        {
            return;
        }

        await _db.InsertManyAsync(drops).ConfigureAwait(false);
    }
}