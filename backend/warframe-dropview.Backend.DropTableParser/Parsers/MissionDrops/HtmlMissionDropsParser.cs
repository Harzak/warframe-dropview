using System.Text.RegularExpressions;

namespace warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;

internal sealed partial class HtmlMissionDropsParser
{
    [GeneratedRegex(@"^(.+?)/(.+?)\s*\((.+?)\)")]
    private static partial Regex HeaderFormat();

    [GeneratedRegex(@"^(.+?)\s*\(([\d.]+)%\)$")]
    private static partial Regex DropInfoFormat();

    private readonly List<HtmlNode> _drops;
    private string _planet;
    private string _mission;
    private string _type;
    private string _currentRotation;

    public bool IsValid { get; private set; }

    public HtmlMissionDropsParser(List<HtmlNode> drops)
    {
        _drops = drops;
        _planet = string.Empty;
        _mission = string.Empty;
        _type = string.Empty;
        _currentRotation = string.Empty;
        this.IsValid = ParseHeader(drops[0].InnerText);
    }

    public List<MissionDrop> Parse()
    {
        List<MissionDrop> allMissionDrops = [];
        if (!IsValid)
        {
            Console.WriteLine("Skipping invalid mission drops: {0}", _drops[0].InnerText);
            return allMissionDrops;
        }
        Console.WriteLine("Parsing mission drops for: {0}/{1} ({2})", _planet, _mission, _type);

        for (int i = 0; i<_drops.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            HtmlNode drop = _drops[i];
            List<HtmlNode> cells = drop.ChildNodes.ToList();
            if (cells.Count == 1 && cells.First().Name == "th")
            {
                _currentRotation = drop.InnerText;
                continue;
            }

            string itemName = cells[0].InnerText.Trim();
            MissionDropItemParser itemParser = new(itemName);
            if (!itemParser.Parse())
            {
                throw new InvalidOperationException("Item drop format is invalid: " + itemName);
            }

            Match match = DropInfoFormat().Match(cells[1].InnerText.Trim());
            if (!match.Success)
            {
                throw new InvalidOperationException("Drop info format is invalid.");
            }
            string rarity = match.Groups[1].Value;
            double percentage = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

            allMissionDrops.Add(new MissionDrop
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Name = itemParser.Name,
                Rarity = rarity,
                DropRate = percentage,
                Type = itemParser.Type.ToString(),
                Subtype = itemParser.SubType,
                Mission = new Mission
                {
                    Name = _mission,
                    Planet = _planet,
                    Type = _type,
                    Rotation = _currentRotation
                }
            });
            Console.WriteLine("Parsed mission drop: {0} ({1} {2}) - {3} ({4}%)", itemParser.Name, itemParser.Type, itemParser.SubType,  rarity, percentage);
        }
        return allMissionDrops;
    }

    private bool ParseHeader(string innerText)
    {
        Match match = HeaderFormat().Match(innerText);
        if (match.Success)
        {
            _planet = match.Groups[1].Value;
            _mission = match.Groups[2].Value;
            _type = match.Groups[3].Value;
        }
        return match.Success;
    }
}

