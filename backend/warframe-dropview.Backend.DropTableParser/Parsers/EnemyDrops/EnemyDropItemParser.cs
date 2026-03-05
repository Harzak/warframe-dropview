namespace warframe_dropview.Backend.DropTableParser.Parsers.EnemyDrops;

/// <summary>
/// Parses raw data representing an enemy drop item and extracts its type, subtype, and name.
/// </summary>
internal sealed class EnemyDropItemParser
{
    private readonly string _rawData;

    /// <summary>
    /// Gets the type of enemy drop associated with the current parsing instance.
    /// </summary>
    public EEnemyDropType Type { get; private set; }

    /// <summary>
    /// Gets the subtype associated with the current parsing instance.
    /// </summary>
    public string SubType { get; private set; }

    /// <summary>
    /// Gets the name associated with the current parsing instance.
    /// </summary>
    public string Name { get; private set; }

    public EnemyDropItemParser(string rawData)
    {
        _rawData = rawData;
        this.Type = EEnemyDropType.Unknown;
        this.SubType = string.Empty;
        this.Name = string.Empty;
    }

    /// <summary>
    /// Parses the raw data string and determines the type and name of the enemy drop.
    /// </summary>
    public bool Parse()
    {
        if (string.IsNullOrWhiteSpace(_rawData))
        {
            return false;
        }
        string[] strings = _rawData.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (strings.Length > 0 && strings[0].Equals("arcane", StringComparison.OrdinalIgnoreCase))
        {
            this.Type =  EEnemyDropType.Arcane;
            this.Name = string.Join(' ', strings[0..]);
            return true;
        }

        if (strings.Length > 1 && strings[1].Equals("endo", StringComparison.OrdinalIgnoreCase))
        {
            this.Type =  EEnemyDropType.Endo;
            this.Name = string.Join(' ', strings[0..]);
            return true;
        }

        this.Type = EEnemyDropType.Mod;
        this.Name = _rawData;
        return true;
    }
}
