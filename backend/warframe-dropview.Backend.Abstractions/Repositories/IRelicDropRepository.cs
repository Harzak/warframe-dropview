namespace warframe_dropview.Backend.Abstractions.Repositories;

public interface IRelicDropRepository
{
    Task<IEnumerable<RelicDrop>> GetDrops(string primeName);
    Task InsertDrops(IEnumerable<RelicDrop> drops);
}