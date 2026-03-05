namespace warframe_dropview.Backend.DropTableParser.Extensions;

/// <summary>
/// Provides extension methods for registering drop table parsers in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDropTableParser(this IServiceCollection services)
    {
        services.AddSingleton<IDropParser, HtmlListDropParser>();
        services.AddSingleton<IDropTableParser<MissionDrop>, HtmlMissionDropsParser>();
        services.AddSingleton<IDropTableParser<EnemyDrop>, HtmlEnemyDropsParser>();
        services.AddSingleton<IDropTableParser<RelicDrop>, HtmlRelicDropsParser>();

        return services;
    }
}