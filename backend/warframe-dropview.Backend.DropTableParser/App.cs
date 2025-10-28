namespace warframe_dropview.Backend.DropTableParser;

internal sealed class App
{
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IDropParser _dropParser;

    public App(IMongoDatabase mongoDatabase, IDropParser dropParser)
    {
        _mongoDatabase = mongoDatabase;
        _dropParser = dropParser;
    }


    public async Task RunAsync()
    {
        await _dropParser.ParseAsync().ConfigureAwait(false);

        Console.ReadKey();
    }
}

