namespace warframe_dropview.Backend.Plugin.MongoDB.Registrations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterMongoDBPlugin(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseSettings settings = configuration?.GetSection("MongoDB").Get<DatabaseSettings>() ?? throw new ArgumentException(nameof(settings));

        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(settings.ConnectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            IMongoClient client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(settings.DatabaseName);
        });

        services.AddScoped<IRelicDropRepository, RelicDropRepository>();

        DocumentsMapping.Map();

        return services;
    }
}