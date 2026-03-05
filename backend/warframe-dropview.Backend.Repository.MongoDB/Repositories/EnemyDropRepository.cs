namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class EnemyDropRepository : IEnemyDropRepository
{
    private readonly IMongoCollection<EnemyDrop> _db;

    public EnemyDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<EnemyDrop>("enemy_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public Task<IEnumerable<EnemyDrop>> SearchDrops(string itemName)
    {
        throw new NotImplementedException();
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