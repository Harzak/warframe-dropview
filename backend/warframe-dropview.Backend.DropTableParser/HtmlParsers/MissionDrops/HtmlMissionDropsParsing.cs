using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using warframe_dropview.Backend.Models;

namespace warframe_dropview.Backend.DropTableParser.HtmlParsers.MissionDrops;

internal sealed partial class HtmlMissionDropsParsing
{
    [GeneratedRegex(@"^(.+?)/(.+?)\s*\((.+?)\)")]
    private static partial Regex HeaderFormat();

    [GeneratedRegex(@"^(.+?)\s*\(([\d.]+)%\)$")]
    private static partial Regex DropInfoFormat();

    private readonly List<HtmlNode> _drops;
    private string _planet = string.Empty;
    private string _mission = string.Empty;
    private string _type = string.Empty;
    private string _currentRotation = string.Empty;

    public bool IsValid { get; private set; }

    public HtmlMissionDropsParsing(List<HtmlNode> drops)
    {
        _drops = drops;
        IsValid = ParseHeader(drops[0].InnerText);
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
                Name = itemName,
                Rarity = rarity,
                DropRate = percentage,
                Subtype = _type,
                Mission = new Mission
                {
                    Name = _mission,
                    Planet = _planet,
                    Type = _type,
                    Rotation = _currentRotation
                }
            });
            Console.WriteLine("Parsed mission drop: {0} - {1} ({2}%)", itemName, rarity, percentage);
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
        //throw new InvalidOperationException("Mission header format is invalid.");
        //todo other template for IE: Duviri/Endless: Tier 1 (Normal)
    }
}

