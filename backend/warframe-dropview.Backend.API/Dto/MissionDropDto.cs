namespace warframe_dropview.Backend.API.Dto;

/// <summary>
/// Represents drop information for a specific mission, including mission name, type, planet, and rotation.
/// </summary>
public class MissionDropDto : BaseDropDto
{
    public string MissionName { get; set; } = string.Empty; //e.g., "Apollodorus", "Lares" ...
    public string MissionType { get; set; } = string.Empty; //e.g., "Assassination", "Defense", "Survival", "Spy", "Sabotage" ...
    public string Planet { get; set; } = string.Empty; //e.g., "Earth", "Mars", "Venus", "Saturn" ...
    public string Rotation { get; set; } = string.Empty; //e.g., "A", "B", "C" (if applicable)
}