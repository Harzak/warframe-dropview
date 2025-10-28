namespace warframe_dropview.Backend.DropTableParser.Parsers.ModBySourceDrops;

internal static class HtmlEnemyListDropsParser
{
    private const string DELIMITER_CLASS = "blank-row";

    public static List<EnemyDrop> Parse(HtmlNode doc)
    {
        HtmlNodeCollection allEnemiesRows = doc.SelectNodes(".//tr");
        List<EnemyDrop> allEnemyDrops = [];

        foreach (List<HtmlNode> enemyDrops in SplitDropsByEnemies(allEnemiesRows))
        {
            HtmlEnemyDropsParser parser = new(enemyDrops);
            allEnemyDrops.AddRange(parser.Parse());
        }

        return allEnemyDrops;
    }

    private static List<List<HtmlNode>> SplitDropsByEnemies(HtmlNodeCollection allEnemiesRows)
    {
        Console.WriteLine("Splitting {0} rows into enemies", allEnemiesRows.Count);

        List<List<HtmlNode>> enemyRowsChunks = [];
        List<HtmlNode> currentChunk = [];

        foreach (HtmlNode row in allEnemiesRows)
        {
            if (row.GetAttributeValue("class", "").Contains(DELIMITER_CLASS, StringComparison.Ordinal))
            {
                enemyRowsChunks.Add(currentChunk);
                currentChunk = [];
                continue;
            }
            currentChunk.Add(row);
        }

        Console.WriteLine("Split {0} rows into {1} enemies", allEnemiesRows.Count, enemyRowsChunks.Count);
        return enemyRowsChunks;
    }
}

