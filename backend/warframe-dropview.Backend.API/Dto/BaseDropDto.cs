namespace warframe_dropview.Backend.API.Dto;

/// <summary>
/// Represents the base data transfer object for a drop, including name, type, drop rate, and rarity.
/// </summary>
public class BaseDropDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; //e.g., "Resource", "Blueprint", "Mod", "Relic", "Warframe Part" ...
    public string Subtype { get; set; } = string.Empty;
    public double DropRate { get; set; }
    public string Rarity { get; set; } = string.Empty; //e.g., "Common", "Uncommon", "Rare", "Legendary" ...
}