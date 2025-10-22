using MongoDB.Driver;

namespace warframe_dropview.Backend.DropTableParser;

internal sealed class App
{
    private readonly IMongoDatabase _mongoDatabase;

    public App(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }


    public async Task RunAsync()
    {
        await DatabaseInitializer.InitializeAsync(_mongoDatabase).ConfigureAwait(false);
    }
}

