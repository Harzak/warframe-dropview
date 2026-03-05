using warframe_dropview.Backend.Abstractions.Repositories;

namespace warframe_dropview.Backend.DropTableParser;

internal sealed class App
{
    private readonly IDropParser _dropParser;
    private readonly IMissionDropRepository _missionDropRepository;
    private readonly IRelicDropRepository _relicDropRepository;
    private readonly IEnemyDropRepository _enemyDropRepository;

    public App(
        IDropParser dropParser, 
        IMissionDropRepository missionDropRepository, 
        IRelicDropRepository relicDropRepository,
        IEnemyDropRepository enemyDropRepository)
    {
        _dropParser = dropParser;
        _missionDropRepository = missionDropRepository;
        _relicDropRepository = relicDropRepository;
        _enemyDropRepository = enemyDropRepository;
    }

    public async Task RunAsync()
    {
        OperationResult parsingResult = await _dropParser.ParseAsync().ConfigureAwait(false);

        if (parsingResult.IsFailed)
        {
            Console.WriteLine("Parsing failed: {0}", parsingResult.ErrorMessage);
            return;
        }

        Console.WriteLine("Parsing succeeded.");
        Console.WriteLine("Inserting drops into the database...");

        Task t1 = _missionDropRepository.InsertDropsAsync(_dropParser.MissionDrops!);
        Task t2 = _relicDropRepository.InsertDropsAsync(_dropParser.RelicDrops!);
        Task t3 = _enemyDropRepository.InsertDropsAsync(_dropParser.EnemyDrops!);

        await Task.WhenAll(t1, t2, t3).ConfigureAwait(false);

        Console.WriteLine("All drops inserted successfully.");

        Console.ReadKey();
    }
}

