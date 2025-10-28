namespace warframe_dropview.Backend.DropTableParser.Parsers.RelicDrops;

internal static class HtmlRelicListDropsParser
{
    private const string RELIC_DELIMITER_CLASS = "blank-row";

    public static List<RelicDrop> Parse(HtmlNode doc)
    {
        HtmlNodeCollection allRelicRows = doc.SelectNodes(".//tr");
        List<RelicDrop> allRelicDrops = [];

        foreach (List<HtmlNode> relicDrops in SplitDropsByRelic(allRelicRows))
        {
            HtmlRelicDropsParser parser = new(relicDrops);
            allRelicDrops.AddRange(parser.Parse());
        }

        return allRelicDrops;
    }

    private static List<List<HtmlNode>> SplitDropsByRelic(HtmlNodeCollection allRelicsRows)
    {
        Console.WriteLine("Splitting {0} rows into relics", allRelicsRows.Count);

        List<List<HtmlNode>> relicRowsChunks = [];
        List<HtmlNode> currentChunk = [];

        foreach (HtmlNode row in allRelicsRows)
        {
            if (row.GetAttributeValue("class", "").Contains(RELIC_DELIMITER_CLASS, StringComparison.Ordinal))
            {
                relicRowsChunks.Add(currentChunk);
                currentChunk = [];
                continue;
            }
            currentChunk.Add(row);
        }

        Console.WriteLine("Split {0} rows into {1} relics", allRelicsRows.Count, relicRowsChunks.Count);
        return relicRowsChunks;
    }
}

