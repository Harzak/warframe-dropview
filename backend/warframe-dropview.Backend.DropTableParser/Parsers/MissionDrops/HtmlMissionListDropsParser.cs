namespace warframe_dropview.Backend.DropTableParser.Parsers.MissionDrops;

internal static class HtmlMissionListDropsParser
{
    private const string MISSION_DELIMITER_CLASS = "blank-row";

    public static List<MissionDrop> Parse(HtmlNode doc)
    {
        HtmlNodeCollection allMissionsRows = doc.SelectNodes(".//tr");
        List<MissionDrop> allMissionDrops = [];

        foreach (List<HtmlNode> missionDrops in SplitDropsByMissions(allMissionsRows))
        {
            HtmlMissionDropsParser parser = new(missionDrops);
            allMissionDrops.AddRange(parser.Parse());
        }

        return allMissionDrops;
    }

    private static List<List<HtmlNode>> SplitDropsByMissions(HtmlNodeCollection allMissionsRows)
    {
        Console.WriteLine("Splitting {0} rows into missions", allMissionsRows.Count);

        List<List<HtmlNode>> missionRowsChunks = [];
        List<HtmlNode> currentChunk = [];

        foreach (HtmlNode row in allMissionsRows)
        {
            if (row.GetAttributeValue("class", "").Contains(MISSION_DELIMITER_CLASS, StringComparison.Ordinal))
            {
                missionRowsChunks.Add(currentChunk);
                currentChunk = [];
                continue;
            }
            currentChunk.Add(row);
        }

        Console.WriteLine("Split {0} rows into {1} missions", allMissionsRows.Count, missionRowsChunks.Count);
        return missionRowsChunks;
    }
}

