using warframe_dropview.Backend.API.Models;

namespace warframe_dropview.Backend.API.Services;

internal static class DatabaseInitializer
{
    public static async Task InitializeAsync(IMongoDatabase database)
    {
        List<string> collectionNames = (await database.ListCollectionNamesAsync().ConfigureAwait(false)).ToList();

        if (!collectionNames.Contains("mission_drops"))
        {
            await database.CreateCollectionAsync("mission_drops").ConfigureAwait(false);
            await CreateMissionDropsIndexesAsync(database).ConfigureAwait(false);
        }

        if (!collectionNames.Contains("relic_drops"))
        {
            await database.CreateCollectionAsync("relic_drops").ConfigureAwait(false);
            await CreateRelicDropsIndexesAsync(database).ConfigureAwait(false);
        }

        if (!collectionNames.Contains("enemy_drops"))
        {
            await database.CreateCollectionAsync("enemy_drops").ConfigureAwait(false);
            await CreateEnemyDropIndexesAsync(database).ConfigureAwait(false);
        }
    }

    private static async Task CreateMissionDropsIndexesAsync(IMongoDatabase database)
    {
        IMongoCollection<MissionDrop> missionDrops = database.GetCollection<MissionDrop>("mission_drops");

        IndexKeysDefinition<MissionDrop> nameIndex = Builders<MissionDrop>.IndexKeys
            .Ascending(d => d.Name);
        IndexKeysDefinition<MissionDrop> compoundIndex = Builders<MissionDrop>.IndexKeys
            .Ascending(d => d.Mission.Type);

        await Task.WhenAll(
           missionDrops.Indexes.CreateOneAsync(new CreateIndexModel<MissionDrop>(nameIndex)),
           missionDrops.Indexes.CreateOneAsync(new CreateIndexModel<MissionDrop>(compoundIndex))
        ).ConfigureAwait(false);
    }

    private static async Task CreateRelicDropsIndexesAsync(IMongoDatabase database)
    {
        IMongoCollection<RelicDrop> relicDrops = database.GetCollection<RelicDrop>("relic_drops");

        IndexKeysDefinition<RelicDrop> nameIndex = Builders<RelicDrop>.IndexKeys
            .Ascending(d => d.Name);
        IndexKeysDefinition<RelicDrop> compoundIndex = Builders<RelicDrop>.IndexKeys
            .Ascending(d => d.Name)
            .Ascending(d => d.Relic.Type);

        await Task.WhenAll(
            relicDrops.Indexes.CreateOneAsync(new CreateIndexModel<RelicDrop>(nameIndex)),
            relicDrops.Indexes.CreateOneAsync(new CreateIndexModel<RelicDrop>(compoundIndex))
        ).ConfigureAwait(false);
    }

    private static async Task CreateEnemyDropIndexesAsync(IMongoDatabase database)
    {
        IMongoCollection<EnemyDrop> enemyDrops = database.GetCollection<EnemyDrop>("enemy_drops");

        IndexKeysDefinition<EnemyDrop> nameIndex = Builders<EnemyDrop>.IndexKeys
            .Ascending(d => d.Name);
        IndexKeysDefinition<EnemyDrop> compoundIndex = Builders<EnemyDrop>.IndexKeys
            .Ascending(d => d.Name)
            .Ascending(d => d.Type);

        await enemyDrops.Indexes.CreateOneAsync(
            new CreateIndexModel<EnemyDrop>(nameIndex)).ConfigureAwait(false);

        await Task.WhenAll(
           enemyDrops.Indexes.CreateOneAsync(new CreateIndexModel<EnemyDrop>(nameIndex)),
           enemyDrops.Indexes.CreateOneAsync(new CreateIndexModel<EnemyDrop>(compoundIndex))
        ).ConfigureAwait(false);
    }
}

