namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IMissionDropRepository
{
    Task<IEnumerable<MissionDrop>> SearchDropsAsync(string itemName,
        string? dropRarities,
        string? itemTypes,
        string? itemSubTypes,
        string? missionTypes,
        int? offset,
        int? limit);
    Task InsertDropsAsync(IEnumerable<MissionDrop> drops);
}