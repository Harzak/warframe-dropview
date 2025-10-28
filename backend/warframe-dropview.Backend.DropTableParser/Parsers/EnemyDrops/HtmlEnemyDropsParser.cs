using System.Numerics;
using System.Text.RegularExpressions;
using warframe_dropview.Backend.DropTableParser.Parsers.EnemyDrops;
using warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;
using warframe_dropview.Backend.Models;

namespace warframe_dropview.Backend.DropTableParser.Parsers.ModBySourceDrops;

internal sealed partial class HtmlEnemyDropsParser
{
    [GeneratedRegex(@"Mod Drop Chance:\s*([\d.]+)%")]
    private static partial Regex HeaderFormat();

    [GeneratedRegex(@"^(.+?)\s*\(([\d.]+)%\)$")]
    private static partial Regex DropInfoFormat();

    private readonly List<HtmlNode> _drops;
    private string _name;
    private decimal _modDropChance;

    public bool IsValid { get; private set; }

    public HtmlEnemyDropsParser(List<HtmlNode> drops)
    {
        _drops = drops;
        _name = string.Empty;
        this.IsValid = ParseHeader(drops[0]);
    }

    public List<EnemyDrop> Parse()
    {
        List<EnemyDrop> allEnemiesDrops = [];
        if (!IsValid)
        {
            Console.WriteLine("Skipping invalid enemy drops: {0}", _drops[0].InnerText);
            return allEnemiesDrops;
        }
        Console.WriteLine("Parsing enemy drops for: {0}", _name);

        for (int i = 0; i<_drops.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            HtmlNode drop = _drops[i];
            List<HtmlNode> cells = drop.ChildNodes.ToList();

            string itemName = cells[1].InnerText.Trim();
            EnemyDropItemParser itemParser = new(itemName);
            if (!itemParser.Parse())
            {
                throw new InvalidOperationException("Item drop format is invalid: " + itemName);
            }

            Match match = DropInfoFormat().Match(cells[2].InnerText.Trim());
            if (!match.Success)
            {
                throw new InvalidOperationException("Drop info format is invalid.");
            }
            string rarity = match.Groups[1].Value;
            double percentage = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);

            allEnemiesDrops.Add(new EnemyDrop
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow,
                Name = itemParser.Name,
                Rarity = rarity,
                DropRate = percentage,
                Type = itemParser.Type.ToString(),
                Enemy = new Enemy
                {
                    Name = _name,
                    ModDropRate = _modDropChance.ToString(CultureInfo.InvariantCulture)
                }
            });
            Console.WriteLine("Parsed enemy drop: {0} ({1} {2}) - {3} ({4}%)", itemParser.Name, itemParser.Type, itemParser.SubType, rarity, percentage);
        }
        return allEnemiesDrops;
    }

    private bool ParseHeader(HtmlNode node)
    {
        if (node.ChildNodes.Count == 0 || string.IsNullOrWhiteSpace(node.ChildNodes[0].InnerText))
        {
            return false;
        }
        _name = node.ChildNodes[0].InnerText;

        Match match = HeaderFormat().Match(node.ChildNodes[1].InnerText);
        if (match.Success)
        {
            _modDropChance = decimal.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        }
        return match.Success;
    }
}

