namespace warframe_dropview.Backend.API.Models;

internal sealed class Enemy
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("modDropRate")]
    public string ModDropRate { get; set; } = string.Empty;
}
