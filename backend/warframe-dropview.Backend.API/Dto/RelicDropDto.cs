using System.Text.Json.Serialization;
namespace warframe_dropview.Backend.API.Dto;

/// <summary>
/// Represents a data transfer object for a specific part drop from a Warframe relic
/// </summary>
public sealed class RelicDropDto : BaseDropDto
{
    public string PartType { get; set; } = string.Empty; //e.g., "Blueprint", "Neuroptics", "Chassis", "Systems", "Barrel" ...
    public string RelicTier { get; set; } = string.Empty; //e.g., "Lith", "Meso", "Neo", "Axi", "Requiem"...
    public string RelicCode { get; set; } = string.Empty; //e.g., "A1", "B2", "S3", "A1", "Requiem S"...
    public string Refinement { get; set; } = string.Empty;//e.g., "Intact", "Exceptional", "Flawless", "Radiant"
}