namespace warframe_dropview.Backend.DropTableParser;

internal sealed class App
{
    private readonly IMongoDatabase _mongoDatabase;

    public App(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }


    public Task RunAsync()
    {
        var doc = new HtmlDocument();
        doc.Load(@"C:\Users\Aurelien\Desktop\Warframe PC Drops.html");

        HtmlNode missionTable = doc.DocumentNode.SelectSingleNode("//table[1]");
        List<MissionDrop> allMissionDrops = HtmlMissionListDropsParser.Parse(missionTable);

        HtmlNode relicTable = doc.DocumentNode.SelectSingleNode("//table[2]");
        List<RelicDrop> allRelicDrops = HtmlRelicListDropsParser.Parse(relicTable);

        HtmlNode enemyTable = doc.DocumentNode.SelectSingleNode("//table[12]");
        List<EnemyDrop> allEnemyDrops = HtmlEnemyListDropsParser.Parse(enemyTable);

        Console.ReadKey();
        return Task.CompletedTask;
    }
}

