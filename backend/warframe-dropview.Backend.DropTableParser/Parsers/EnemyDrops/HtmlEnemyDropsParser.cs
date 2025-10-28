using System.Text.RegularExpressions;

namespace warframe_dropview.Backend.DropTableParser.Parsers.EnemyDrops;

internal sealed partial class HtmlEnemyDropsParser : BaseHtmlDropParser<EnemyDrop>
{
    [GeneratedRegex(@"Mod Drop Chance:\s*([\d.]+)%")]
    private static partial Regex HeaderFormat();

    private string _name;
    private decimal _modDropChance;

    public HtmlEnemyDropsParser(List<HtmlNode> drops) : base(drops)
    {
        _name = string.Empty;
    }

    protected override List<EnemyDrop> ParseInternal()
    {
        List<EnemyDrop> allEnemiesDrops = [];
        Console.WriteLine("Parsing enemy drops for: {0}", _name);

        for (int i = 0; i<base.Drops.Count; i++)
        {
            if (i == 0)
            {
                continue;
            }

            HtmlNode drop = base.Drops[i];
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

    protected override bool ParseHeader(HtmlNode node)
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

