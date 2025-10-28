namespace warframe_dropview.Backend.DropTableParser.Parsers;

internal sealed class HtmlListDropParser : IDropParser
{
    private const string DELIMITER_CLASS = "blank-row";

    public ReadOnlyCollection<MissionDrop>? MissionDrops { get; private set; }
    public ReadOnlyCollection<RelicDrop>? RelicDrops { get; private set; }
    public ReadOnlyCollection<EnemyDrop>? EnemyDrops { get; private set; }

    public async Task ParseAsync()
    {
        HtmlDocument doc = new();
        doc.Load(@"C:\Users\Aurelien\Desktop\Warframe PC Drops.html");

        Task<ReadOnlyCollection<MissionDrop>> missionTask = Task.Run(() => ParseMissions(doc.DocumentNode.SelectSingleNode("//table[1]")).AsReadOnly());
        Task<ReadOnlyCollection<RelicDrop>> relicTask = Task.Run(() => ParseRelics(doc.DocumentNode.SelectSingleNode("//table[2]")).AsReadOnly());
        Task<ReadOnlyCollection<EnemyDrop>> enemyTask = Task.Run(() => ParseEnemies(doc.DocumentNode.SelectSingleNode("//table[12]")).AsReadOnly());

        await Task.WhenAll(missionTask, relicTask, enemyTask).ConfigureAwait(false);

        this.MissionDrops = await missionTask.ConfigureAwait(false);
        this.RelicDrops = await relicTask.ConfigureAwait(false);
        this.EnemyDrops = await enemyTask.ConfigureAwait(false);
    }

    private List<MissionDrop> ParseMissions(HtmlNode table) => ParseDrops(table, nodes => new HtmlMissionDropsParser(nodes));

    private List<RelicDrop> ParseRelics(HtmlNode table) => ParseDrops(table, nodes => new HtmlRelicDropsParser(nodes));

    private List<EnemyDrop> ParseEnemies(HtmlNode table) => ParseDrops(table, nodes => new HtmlEnemyDropsParser(nodes));

    private List<T> ParseDrops<T>(HtmlNode doc, Func<List<HtmlNode>, IDropTableParser<T>> parserFactory) where T : class
    {
        HtmlNodeCollection allRows = doc.SelectNodes(".//tr");
        List<T> allDrops = [];

        foreach (List<HtmlNode> drops in SplitRowsIntoChunks(allRows))
        {
            using var parser = parserFactory.Invoke(drops);
            ReadOnlyCollection<T>? parsedDrop = parser.Parse();
            if (parsedDrop != null)
            {
                allDrops.AddRange(parsedDrop);
            }
        }
        return allDrops;
    }

    private List<List<HtmlNode>> SplitRowsIntoChunks(HtmlNodeCollection allEnemiesRows)
    {
        Console.WriteLine("Splitting {0} rows into enemies", allEnemiesRows.Count);

        List<List<HtmlNode>> enemyRowsChunks = [];
        List<HtmlNode> currentChunk = [];

        foreach (HtmlNode row in allEnemiesRows)
        {
            if (row.GetAttributeValue("class", "").Contains(DELIMITER_CLASS, StringComparison.Ordinal))
            {
                enemyRowsChunks.Add(currentChunk);
                currentChunk = [];
                continue;
            }
            currentChunk.Add(row);
        }

        Console.WriteLine("Split {0} rows into {1} enemies", allEnemiesRows.Count, enemyRowsChunks.Count);
        return enemyRowsChunks;
    }
}