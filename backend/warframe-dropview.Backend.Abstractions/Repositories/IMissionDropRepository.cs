namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IMissionDropRepository
{
    Task<IEnumerable<MissionDrop>> GetDrops(string itemName);
    Task InsertDrops(IEnumerable<MissionDrop> drops);
}