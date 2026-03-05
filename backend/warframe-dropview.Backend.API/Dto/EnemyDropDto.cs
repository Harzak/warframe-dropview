namespace warframe_dropview.Backend.API.Dto;

/// <summary>
/// Represents drop information specific to an enemy.
/// </summary>
public class EnemyDropDto : BaseDropDto
{
    public string EnemyName { get; set; } = string.Empty; //e.g., "Grineer Lancer", "Corpus Tech", "Infested Charger" ...
}