

namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class EnemyDropRepository : IEnemyDropRepository
{
    public Task<IEnumerable<EnemyDrop>> GetDrops(string itemName)
    {
        throw new NotImplementedException();
    }

    public Task InsertDrops(IEnumerable<EnemyDrop> drops)
    {
        throw new NotImplementedException();
    }
}

