namespace warframe_dropview.Backend.Models;

public sealed class MissionDrop : Drop
{
    public string Subtype { get; set; } = string.Empty;
    public Mission Mission { get; set; } = new();
}