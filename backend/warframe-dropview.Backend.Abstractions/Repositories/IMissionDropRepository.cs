namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IMissionDropRepository
{
    Task<IEnumerable<MissionDrop>> SearchDropsAsync(string? itemName, int? offset, int? limit);
    Task InsertDropsAsync(IEnumerable<MissionDrop> drops);
}