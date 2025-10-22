using Microsoft.Extensions.Configuration;
using warframe_dropview.Backend.Models;

namespace warframe_dropview.Backend.DropTableParser;

internal sealed class Program
{
    static async Task Main(string[] args)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true);

        builder.Services.RegisterMongoDBPlugin(builder.Configuration);
        builder.Services.AddSingleton<App>();

        App app = builder.Build().Services.GetRequiredService<App>();

        await app.RunAsync().ConfigureAwait(false);
    }
}