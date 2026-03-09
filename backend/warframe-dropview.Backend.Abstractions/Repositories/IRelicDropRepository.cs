namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IRelicDropRepository
{
    Task<IEnumerable<RelicDrop>> SearchDropsAsync(
        string itemName,
        string? relicTiers,
        string? dropRarities,
        string? refinement,
        int? offset,
        int? limit);

    Task InsertDropsAsync(IEnumerable<RelicDrop> drops);
}