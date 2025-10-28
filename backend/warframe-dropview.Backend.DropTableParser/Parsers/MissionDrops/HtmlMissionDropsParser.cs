using System.Text.RegularExpressions;

namespace warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;

internal sealed partial class HtmlMissionDropsParser : BaseHtmlDropParser<MissionDrop>
{
    [GeneratedRegex(@"^(.+?)/(.+?)\s*\((.+?)\)")]
    private static partial Regex HeaderFormat();

    private string _planet;
    private string _mission;
    private string _type;
    private string _currentRotation;

    public HtmlMissionDropsParser(List<HtmlNode> drops, ILogger<HtmlMissionDropsParser> logger) : base(drops, logger)
    {
        _planet = string.Empty;
        _mission = string.Empty;
        _type = string.Empty;
        _currentRotation = string.Empty;
    }

    protected override List<MissionDrop> ParseInternal()
    {
        List<MissionDrop> allMissionDrops = [];
        base.Logger.LogParsingMissionDrops(_planet, _mission, _type);

        for (int i = 0; i<base.Drops.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            HtmlNode drop = base.Drops[i];
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
            base.Logger.LogParsedMissionDrop(itemParser.Name, itemParser.Type.ToString(), itemParser.SubType, rarity, percentage);
        }
        return allMissionDrops;
    }

    protected override bool ParseHeader(HtmlNode node)
    {
        Match match = HeaderFormat().Match(node.InnerText);
        if (match.Success)
        {
            _planet = match.Groups[1].Value;
            _mission = match.Groups[2].Value;
            _type = match.Groups[3].Value;
        }
        return match.Success;
    }
}

