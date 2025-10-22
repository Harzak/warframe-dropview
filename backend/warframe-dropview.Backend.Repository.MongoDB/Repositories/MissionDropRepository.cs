
namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class MissionDropRepository : IMissionDropRepository
{
    public Task<IEnumerable<MissionDrop>> GetDrops(string itemName)
    {
        throw new NotImplementedException();
    }

    public Task InsertDrops(IEnumerable<MissionDrop> drops)
    {
        throw new NotImplementedException();
    }
}

