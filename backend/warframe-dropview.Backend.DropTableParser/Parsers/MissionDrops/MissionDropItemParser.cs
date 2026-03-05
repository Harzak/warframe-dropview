namespace warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;

/// <summary>
/// Parses raw mission drop data to extract and classify information about the drop item.
/// </summary>
internal sealed class MissionDropItemParser
{
    private readonly string _rawData;

    /// <summary>
    /// Gets the type of mission drop associated with the current parsing instance.
    /// </summary>
    public EMissionDropType Type { get; private set; }

    /// <summary>
    /// Gets the subtype associated with the current parsing instance.
    /// </summary>
    public string SubType { get; private set; }

    /// <summary>
    /// Gets the name associated with the current parsing instance.
    /// </summary>
    public string Name { get; private set; }

    public MissionDropItemParser(string rawData)
    {
        _rawData = rawData;
        this.Type = EMissionDropType.Unknown;
        this.SubType = string.Empty;
        this.Name = string.Empty;
    }

    /// <summary>
    /// Parses the raw data string and determines the type, name, and subtype of the mission drop.
    /// </summary>
    public bool Parse()
    {
        if (string.IsNullOrWhiteSpace(_rawData))
        {
            return false;
        }
        string[] strings = _rawData.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (strings.Length == 3 && strings[2].Equals("relic", StringComparison.OrdinalIgnoreCase))
        {
            this.Type = EMissionDropType.Relic;
            this.Name = strings[1];
            this.SubType = strings[0];
            return true;
        }

        if (strings.Any(x => x.Equals("blueprint", StringComparison.OrdinalIgnoreCase)))
        {
            this.Type = EMissionDropType.Blueprint;
            int blueprintIndex = Array.FindIndex(strings, x => x.Equals("blueprint", StringComparison.OrdinalIgnoreCase));
            this.Name = string.Join(' ', strings[..blueprintIndex]);
            return true;
        }

        if (strings.Length > 0 && strings[0].Equals("arcane", StringComparison.OrdinalIgnoreCase))
        {
            this.Type = EMissionDropType.Arcane;
            this.Name = string.Join(' ', strings[0..]);
            return true;
        }

        if (strings.Length > 0 && strings[0].Contains('X', StringComparison.OrdinalIgnoreCase))
        {
            this.Type = EMissionDropType.Resource;
            this.Name = string.Join(' ', strings[0..]);
            return true;
        }

        if (strings.Length > 0 &&  int.TryParse(strings[0], NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int number))
        {
            this.Type = EMissionDropType.Resource;
            this.Name = string.Join(' ', strings[0..]);
            return true;
        }

        this.Type = EMissionDropType.Mod;
        this.Name = _rawData;

        return true;
    }
}

