namespace warframe_dropview.Backend.Models;

public abstract class Drop
{
    public string Id { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double DropRate { get; set; }
    public string Rarity { get; set; } = string.Empty;
}