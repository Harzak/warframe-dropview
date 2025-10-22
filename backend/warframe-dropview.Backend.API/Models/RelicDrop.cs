namespace warframe_dropview.Backend.API.Models;

internal sealed class RelicDrop : Drop
{
    [BsonElement("subtype")]
    [BsonIgnoreIfNull]
    public string Subtype { get; set; } = string.Empty;

    [BsonElement("relic")]
    public Relic Relic { get; set; } = new();
}

