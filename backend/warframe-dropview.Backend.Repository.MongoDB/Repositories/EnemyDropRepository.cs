namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class EnemyDropRepository : IEnemyDropRepository
{
    private readonly IMongoCollection<EnemyDrop> _db;

    public EnemyDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<EnemyDrop>("enemy_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<IEnumerable<EnemyDrop>> SearchDrops(string itemName)
    {
        FilterDefinitionBuilder<EnemyDrop> builder = Builders<EnemyDrop>.Filter;
        FilterDefinition<EnemyDrop  > filter = builder.Empty;

        if (!string.IsNullOrWhiteSpace(itemName))
        {
            filter &= builder.Eq(d => d.Name, itemName);
        }

        IAsyncCursor<EnemyDrop> cursor = await _db.FindAsync(filter).ConfigureAwait(false);
        List<EnemyDrop> results = await cursor.ToListAsync().ConfigureAwait(false);

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