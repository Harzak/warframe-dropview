namespace warframe_dropview.Backend.DropTableParser.Parsers;

internal sealed partial class HtmlListDropParser : IDropParser
{
    private const string DELIMITER_CLASS = "blank-row";

    private readonly ILogger<HtmlListDropParser> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public ReadOnlyCollection<MissionDrop>? MissionDrops { get; private set; }
    public ReadOnlyCollection<RelicDrop>? RelicDrops { get; private set; }
    public ReadOnlyCollection<EnemyDrop>? EnemyDrops { get; private set; }

    public HtmlListDropParser(ILogger<HtmlListDropParser> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    public async Task ParseAsync()
    {
        HtmlDocument doc = new();
        string docPath = Path.Combine("Resources", "Warframe-PC-Drops_2025_10_21.html");
        doc.Load(docPath);

        Task<ReadOnlyCollection<MissionDrop>> missionTask = Task.Run(() => ParseMissions(doc.DocumentNode.SelectSingleNode("//table[1]")).AsReadOnly());
        Task<ReadOnlyCollection<RelicDrop>> relicTask = Task.Run(() => ParseRelics(doc.DocumentNode.SelectSingleNode("//table[2]")).AsReadOnly());
        Task<ReadOnlyCollection<EnemyDrop>> enemyTask = Task.Run(() => ParseEnemies(doc.DocumentNode.SelectSingleNode("//table[12]")).AsReadOnly());

        await Task.WhenAll(missionTask, relicTask, enemyTask).ConfigureAwait(false);

        this.MissionDrops = await missionTask.ConfigureAwait(false);
        this.RelicDrops = await relicTask.ConfigureAwait(false);
        this.EnemyDrops = await enemyTask.ConfigureAwait(false);
    }

    private List<MissionDrop> ParseMissions(HtmlNode table) 
        => ParseDrops(table, nodes => new HtmlMissionDropsParser(nodes, _loggerFactory.CreateLogger<HtmlMissionDropsParser>()));

    private List<RelicDrop> ParseRelics(HtmlNode table) 
        => ParseDrops(table, nodes => new HtmlRelicDropsParser(nodes, _loggerFactory.CreateLogger<HtmlRelicDropsParser>()));

    private List<EnemyDrop> ParseEnemies(HtmlNode table) 
        => ParseDrops(table, nodes => new HtmlEnemyDropsParser(nodes, _loggerFactory.CreateLogger<HtmlEnemyDropsParser>()));

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
        _logger.LogSplittingRows(allEnemiesRows.Count);

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

        _logger.LogSplitComplete(allEnemiesRows.Count, enemyRowsChunks.Count);
        return enemyRowsChunks;
    }
}