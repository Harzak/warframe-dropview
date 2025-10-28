namespace warframe_dropview.Backend.Plugin.MongoDB.Mappings;

internal static class DocumentsMapping
{
    public static void Map()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Drop)))
        {
            BsonClassMap.RegisterClassMap<Drop>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(x => x.Id);
                cm.SetIsRootClass(true);
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(MissionDrop)))
        {
            BsonClassMap.RegisterClassMap<MissionDrop>(cm =>
            {
                cm.AutoMap();
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(RelicDrop)))
        {
            BsonClassMap.RegisterClassMap<RelicDrop>(cm =>
            {
                cm.AutoMap();
            });
        }

        if (!BsonClassMap.IsClassMapRegistered(typeof(EnemyDrop)))
        {
            BsonClassMap.RegisterClassMap<EnemyDrop>(cm =>
            {
                cm.AutoMap();
            });
        }
    }
}

