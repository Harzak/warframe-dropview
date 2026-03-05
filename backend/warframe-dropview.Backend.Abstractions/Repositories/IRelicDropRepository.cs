namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IRelicDropRepository
{
    Task<IEnumerable<RelicDrop>> SearchDropsAsync(
        string? dropType,
        string? partType,
        string? relicTier,
        string? dropRarity);

    Task InsertDropsAsync(IEnumerable<RelicDrop> drops);
}