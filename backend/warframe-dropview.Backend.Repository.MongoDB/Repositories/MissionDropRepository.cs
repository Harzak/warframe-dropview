namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class MissionDropRepository : IMissionDropRepository
{
    private readonly IMongoCollection<MissionDrop> _db;

    public MissionDropRepository(IMongoDatabase database)
    {
        _db = database.GetCollection<MissionDrop>("mission_drops") ?? throw new ArgumentNullException(nameof(database));
    }

    public Task<IEnumerable<MissionDrop>> SearchDropsAsync(string itemName)
    {
        throw new NotImplementedException();
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

