namespace warframe_dropview.Backend.Models;

public sealed class RelicDrop : Drop
{
    public string Subtype { get; set; } = string.Empty;
    public Relic Relic { get; set; } = new();
}

