namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IEnemyDropRepository
{
    Task<IEnumerable<EnemyDrop>> SearchDropsAsync(string itemName,
        string? dropRarities,
        string? itemTypes,
        int? offset,
        int? limit);
    Task InsertDropsAsync(IEnumerable<EnemyDrop> drops);
}