using Microsoft.Extensions.Configuration;

namespace warframe_dropview.Backend.DropTableParser;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true);

        // Configure logging for better performance
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Logging.AddSimpleConsole(options =>
        {
            options.SingleLine = true;
            options.TimestampFormat = null;
            options.IncludeScopes = false;
        });

        builder.Services.RegisterMongoDBPlugin(builder.Configuration)
                        .RegisterDropTableParser()
                        .AddSingleton<App>();

        App app = builder.Build().Services.GetRequiredService<App>();

        await app.RunAsync().ConfigureAwait(false);
    }
}