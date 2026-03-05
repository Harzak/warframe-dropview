using System.Collections.ObjectModel;

namespace warframe_dropview.Backend.API.Dto;

/// <summary>
/// Represents the results of a search, including relic, mission, and enemy drop information when applicable.
/// </summary>
public class SearchResultDto
{
    public Collection<RelicDropDto> RelicDrops { get; } = [];
    public Collection<MissionDropDto> MissionDrops { get; } = [];
    public Collection<EnemyDropDto> EnemyDrops { get; } = [];
}