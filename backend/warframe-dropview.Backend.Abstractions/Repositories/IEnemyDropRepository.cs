namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IEnemyDropRepository
{
    Task<IEnumerable<EnemyDrop>> SearchDrops(string itemName);
    Task InsertDropsAsync(IEnumerable<EnemyDrop> drops);
}