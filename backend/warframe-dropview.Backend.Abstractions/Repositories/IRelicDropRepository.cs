namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IRelicDropRepository
{
    Task<IEnumerable<RelicDrop>> SearchDropsAsync(
        string itemName,
        string? dropType,
        string? partType,
        string? relicTier,
        string? dropRarity,
        int? offset,
        int? limit);

    Task InsertDropsAsync(IEnumerable<RelicDrop> drops);
}