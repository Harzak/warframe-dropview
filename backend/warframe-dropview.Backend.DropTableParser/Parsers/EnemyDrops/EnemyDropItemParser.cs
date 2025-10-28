namespace warframe_dropview.Backend.DropTableParser.Parsers.EnemyDrops;

internal sealed class EnemyDropItemParser
{
    private readonly string _rawData;

    public EEnemyDropType Type { get; private set; }
    public string SubType { get; private set; }
    public string Name { get; private set; }

    public EnemyDropItemParser(string rawData)
    {
        _rawData = rawData;
        this.Type = EEnemyDropType.Unknown;
        this.SubType = string.Empty;
        this.Name = string.Empty;
    }

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
