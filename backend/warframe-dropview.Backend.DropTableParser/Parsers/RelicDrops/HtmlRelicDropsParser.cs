using System.Numerics;
using System.Text.RegularExpressions;
using warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;
using warframe_dropview.Backend.Models;

namespace warframe_dropview.Backend.DropTableParser.Parsers.RelicDrops;

internal sealed partial class HtmlRelicDropsParser
{
    [GeneratedRegex(@"^(\w+)\s+(\w+)\s+Relic\s*\((.+?)\)")]
    private static partial Regex HeaderFormat();


    [GeneratedRegex(@"^(.+?)\s*\(([\d.]+)%\)$")]
    private static partial Regex DropInfoFormat();

    private readonly List<HtmlNode> _drops;
    private string _tier;
    private string _code;
    private string _refinement;

    public bool IsValid { get; private set; }

    public HtmlRelicDropsParser(List<HtmlNode> drops)
    {
        _drops = drops;
        _tier = string.Empty;
        _code = string.Empty;
        _refinement = string.Empty;
        this.IsValid = ParseHeader(drops[0].InnerText);
    }

    public List<RelicDrop> Parse()
    {
        List<RelicDrop> allRelicDrops = [];
        if (!IsValid || _tier.Equals("requiem", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Skipping invalid relic drops: {0}", _drops[0].InnerText);
            return allRelicDrops;
        }
        Console.WriteLine("Parsing relic drops for: {0} {1} ({2})", _tier, _code, _refinement);

        for (int i = 0; i<_drops.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            HtmlNode drop = _drops[i];
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

    private bool ParseHeader(string innerText)
    {
        Match match = HeaderFormat().Match(innerText);

        if (match.Success)
        {
            _tier = match.Groups[1].Value;
            _code = match.Groups[2].Value;
            _refinement = match.Groups[3].Value;
        }
        return match.Success;
    }

}

