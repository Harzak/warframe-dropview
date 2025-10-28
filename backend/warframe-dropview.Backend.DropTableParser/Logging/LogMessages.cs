namespace warframe_dropview.Backend.DropTableParser.Logging;

internal static partial class LogMessages
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Skipping invalid drops: {DropInfo}")]
    public static partial void LogSkippingInvalidDrops(this ILogger logger, string dropInfo);

    [LoggerMessage(Level = LogLevel.Information, Message = "Skipping invalid relic drops: {DropInfo}")]
    public static partial void LogSkippingInvalidRelicDrops(this ILogger logger, string dropInfo);

    [LoggerMessage(Level = LogLevel.Information, Message = "Parsing relic drops for: {Tier} {Code} ({Refinement})")]
    public static partial void LogParsingRelicDrops(this ILogger logger, string tier, string code, string refinement);

    [LoggerMessage(Level = LogLevel.Information, Message = "Parsed relic drop: {Name} ({Type} {SubType}) - {Rarity} ({Percentage}%)")]
    public static partial void LogParsedRelicDrop(this ILogger logger, string name, string type, string subType, string rarity, double percentage);

    [LoggerMessage(Level = LogLevel.Information, Message = "Parsing mission drops for: {Planet}/{Mission} ({Type})")]
    public static partial void LogParsingMissionDrops(this ILogger logger, string planet, string mission, string type);

    [LoggerMessage(Level = LogLevel.Information, Message = "Parsed mission drop: {Name} ({Type} {SubType}) - {Rarity} ({Percentage}%)")]
    public static partial void LogParsedMissionDrop(this ILogger logger, string name, string type, string subType, string rarity, double percentage);

    [LoggerMessage(Level = LogLevel.Information, Message = "Parsing enemy drops for: {Name}")]
    public static partial void LogParsingEnemyDrops(this ILogger logger, string name);

    [LoggerMessage(Level = LogLevel.Information, Message = "Parsed enemy drop: {Name} ({Type} {SubType}) - {Rarity} ({Percentage}%)")]
    public static partial void LogParsedEnemyDrop(this ILogger logger, string name, string type, string subType, string rarity, double percentage);

    [LoggerMessage(Level = LogLevel.Information, Message = "Splitting {Count} rows into enemies")]
    public static partial void LogSplittingRows(this ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Split {RowCount} rows into {EnemyCount} enemies")]
    public static partial void LogSplitComplete(this ILogger logger, int rowCount, int enemyCount);
}