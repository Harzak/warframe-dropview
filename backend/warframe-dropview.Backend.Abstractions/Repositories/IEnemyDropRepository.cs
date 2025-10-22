namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IEnemyDropRepository
{
    Task<IEnumerable<EnemyDrop>> GetDrops(string itemName);
    Task InsertDrops(IEnumerable<EnemyDrop> drops);
}