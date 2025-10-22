namespace warframe_dropview.Backend.API.Models;

internal sealed class MissionDrop : Drop
{
    [BsonElement("subtype")]
    [BsonIgnoreIfNull]
    public string Subtype { get; set; } = string.Empty;

    [BsonElement("mission")]
    public Mission Mission { get; set; } = new();
}