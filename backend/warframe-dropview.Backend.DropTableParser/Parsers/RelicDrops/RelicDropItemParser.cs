namespace warframe_dropview.Backend.DropTableParser.Parsers.RelicDrops;

internal sealed class RelicDropItemParser
{
    private readonly string _rawData;

    public ERelicDropType Type { get; private set; }
    public string SubType { get; private set; }
    public string Name { get; private set; }

    public RelicDropItemParser(string rawData)
    {
        _rawData = rawData;
        Type = ERelicDropType.Unknown;
        SubType = string.Empty;
        Name = string.Empty;
    }

    public bool Parse()
    {
        if (string.IsNullOrWhiteSpace(_rawData))
        {
            return false;
        }
        string[] strings = _rawData.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (strings.Length > 0 && strings[0].Equals("forma", StringComparison.OrdinalIgnoreCase))
        {
           this.Type = ERelicDropType.Forma;
           this.Name = _rawData;
            return true;
        }

        if (strings.Length > 2 && new List<string>() { "SYSTEMS", "CHASSIS", "NEUROPTICS" }.Contains(strings[2].ToUpperInvariant()))
        {
            this.Type = ERelicDropType.WarframePart;
            this.Name = _rawData;
            this.SubType = strings[2];
            return true;
        }

        if (strings.Any(x => x.Equals("blueprint", StringComparison.OrdinalIgnoreCase)))
        {
            Type = ERelicDropType.Blueprint;
            int blueprintIndex = Array.FindIndex(strings, x => x.Equals("blueprint", StringComparison.OrdinalIgnoreCase));
            Name = string.Join(' ', strings[..blueprintIndex]);
            return true;
        }

        this.Type = ERelicDropType.WeaponPart;
        this.Name = _rawData;
        int primeIndex = Array.FindIndex(strings, x => x.Equals("prime", StringComparison.OrdinalIgnoreCase));
        this.SubType = string.Join(' ', strings[(primeIndex+1)..]);

        return true;
    }
}