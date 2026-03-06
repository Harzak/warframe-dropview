namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IMissionDropRepository
{
    Task<IEnumerable<MissionDrop>> SearchDropsAsync(string? itemName);
    Task InsertDropsAsync(IEnumerable<MissionDrop> drops);
}