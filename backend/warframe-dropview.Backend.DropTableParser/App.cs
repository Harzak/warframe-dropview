using System.Globalization;
using System.Text.RegularExpressions;
using warframe_dropview.Backend.DropTableParser.HtmlParsers.MissionDrops;
using warframe_dropview.Backend.Models;

namespace warframe_dropview.Backend.DropTableParser;

internal sealed class App
{
    private readonly IMongoDatabase _mongoDatabase;

    public App(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }


    public Task RunAsync()
    {
        var doc = new HtmlDocument();
        doc.Load(@"C:\Users\Aurelien\Desktop\Warframe PC Drops.html");

        HtmlNode missionTable = doc.DocumentNode.SelectSingleNode("//table[1]");

        List<MissionDrop> allMissionDrops = HtmlMissionsListDropsParser.Parse(missionTable);

        Console.ReadKey();
        return Task.CompletedTask;
    }
}

