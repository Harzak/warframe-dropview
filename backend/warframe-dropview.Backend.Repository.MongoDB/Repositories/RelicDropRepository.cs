namespace warframe_dropview.Backend.Plugin.MongoDB.Repositories;

internal sealed class RelicDropRepository : IRelicDropRepository
{
    public Task<IEnumerable<RelicDrop>> GetDrops(string primeName)
    {
        throw new NotImplementedException();
    }

    public Task InsertDrops(IEnumerable<RelicDrop> drops)
    {
        throw new NotImplementedException();
    }
}

