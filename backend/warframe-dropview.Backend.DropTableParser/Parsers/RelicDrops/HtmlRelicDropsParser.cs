using System.Text.RegularExpressions;

namespace warframe_dropview.Backend.DropTableParser.Parsers.RelicDrops;

internal sealed partial class HtmlRelicDropsParser : BaseHtmlDropParser<RelicDrop>
{
    [GeneratedRegex(@"^(\w+)\s+(\w+)\s+Relic\s*\((.+?)\)")]
    private static partial Regex HeaderFormat();

    private string _tier;
    private string _code;
    private string _refinement;

    public HtmlRelicDropsParser(List<HtmlNode> drops) : base(drops)
    {
        _tier = string.Empty;
        _code = string.Empty;
        _refinement = string.Empty;
    }

    protected override List<RelicDrop> ParseInternal()
    {
        List<RelicDrop> allRelicDrops = [];

        if (_tier.Equals("requiem", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Skipping invalid relic drops: {0}", base.Drops[0].InnerText);
            return allRelicDrops;
        }

        Console.WriteLine("Parsing relic drops for: {0} {1} ({2})", _tier, _code, _refinement);

        for (int i = 0; i<base.Drops.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            HtmlNode drop = base.Drops[i];
            List<HtmlNode> cells = drop.ChildNodes.ToList();

            string itemName = cells[0].InnerText.Trim();
            RelicDropItemParser itemParser = new(itemName);
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

            allRelicDrops.Add(new RelicDrop
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Name = itemParser.Name,
                Rarity = rarity,
                DropRate = percentage,
                Type = itemParser.Type.ToString(),
                Subtype = itemParser.SubType,
                Relic = new Relic
                {
                    Code = _code,
                    Refinement = _refinement,
                    Tier = _tier
                }
            });
            Console.WriteLine("Parsed relic drop: {0} ({1} {2}) - {3} ({4}%)", itemParser.Name, itemParser.Type, itemParser.SubType, rarity, percentage);
        }
        return allRelicDrops;
    }

    protected override bool ParseHeader(HtmlNode node)
    {
        Match match = HeaderFormat().Match(node.InnerText);

        if (match.Success)
        {
            _tier = match.Groups[1].Value;
            _code = match.Groups[2].Value;
            _refinement = match.Groups[3].Value;
        }
        return match.Success;
    }
}