namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IEnemyDropRepository
{
    Task<IEnumerable<EnemyDrop>> SearchDrops(string itemName, int? offset, int? limit);
    Task InsertDropsAsync(IEnumerable<EnemyDrop> drops);
}