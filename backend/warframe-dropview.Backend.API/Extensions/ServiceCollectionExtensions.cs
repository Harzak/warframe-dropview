using warframe_dropview.Backend.API.Models.Settings;

namespace warframe_dropview.Backend.API.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
    }

    public static void ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDBSettings settings = configuration.GetSection("MongoDB").Get<MongoDBSettings>() ?? throw new ArgumentException(nameof(settings));

        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            IMongoClient client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });
    }
}

