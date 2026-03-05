using warframe_dropview.Backend.Abstractions.Results;

namespace warframe_dropview.Backend.DropTableParser.Parsers;

/// <summary>
/// Parses HTML documents to extract drop data for missions, relics, and enemies in a structured format.
/// </summary>
internal sealed partial class HtmlListDropParser : IDropParser
{
    private const string DROP_FILENAME = "Warframe-PC-Drops_2025_10_21.html";
    private const string MISSION_TABLE_NODE_XPATH = "//table[1]";
    private const string RELIC_TABLE_NODE_XPATH = "//table[2]";
    private const string ENEMY_TABLE_NODE_XPATH = "//table[12]";
    private const string DELIMITER_CLASS = "blank-row";

    private readonly ILogger<HtmlListDropParser> _logger;
    private readonly ILoggerFactory _loggerFactory;

    private string DROP_FILEPATH => Path.Combine("Resources", DROP_FILENAME);

    /// <inheritdoc/>
    public ReadOnlyCollection<MissionDrop>? MissionDrops { get; private set; }

    /// <inheritdoc/>
    public ReadOnlyCollection<RelicDrop>? RelicDrops { get; private set; }

    /// <inheritdoc/>
    public ReadOnlyCollection<EnemyDrop>? EnemyDrops { get; private set; }

    public HtmlListDropParser(ILogger<HtmlListDropParser> logger, ILoggerFactory loggerFactory)
    {
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public async Task<OperationResult> ParseAsync()
    {
        HtmlDocument doc = new();
        doc.Load(DROP_FILEPATH);

        Task<ReadOnlyCollection<MissionDrop>> missionTask = Task.Run(() => ParseMissions(doc.DocumentNode.SelectSingleNode(MISSION_TABLE_NODE_XPATH)).AsReadOnly());
        Task<ReadOnlyCollection<RelicDrop>> relicTask = Task.Run(() => ParseRelics(doc.DocumentNode.SelectSingleNode(RELIC_TABLE_NODE_XPATH)).AsReadOnly());
        Task<ReadOnlyCollection<EnemyDrop>> enemyTask = Task.Run(() => ParseEnemies(doc.DocumentNode.SelectSingleNode(ENEMY_TABLE_NODE_XPATH)).AsReadOnly());

        await Task.WhenAll(missionTask, relicTask, enemyTask).ConfigureAwait(false);

        this.MissionDrops = await missionTask.ConfigureAwait(false);
        this.RelicDrops = await relicTask.ConfigureAwait(false);
        this.EnemyDrops = await enemyTask.ConfigureAwait(false);

        return this.GetResult();
    }

    /// <inheritdoc />
    public OperationResult Parse()
    {
        HtmlDocument doc = new();
        doc.Load(DROP_FILEPATH);

        this.MissionDrops = ParseMissions(doc.DocumentNode.SelectSingleNode(MISSION_TABLE_NODE_XPATH)).AsReadOnly();
        this.RelicDrops = ParseRelics(doc.DocumentNode.SelectSingleNode(RELIC_TABLE_NODE_XPATH)).AsReadOnly();
        this.EnemyDrops = ParseEnemies(doc.DocumentNode.SelectSingleNode(ENEMY_TABLE_NODE_XPATH)).AsReadOnly();

       return this.GetResult();
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

    private OperationResult GetResult()
    {
        OperationResult result = new();
        if (this.MissionDrops == null || this.RelicDrops == null || this.EnemyDrops == null)
        {
            result.WithError("One or more drop categories failed to parse.");
        }
        return result.WithSuccess();
    }
}