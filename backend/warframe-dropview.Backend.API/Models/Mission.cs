namespace warframe_dropview.Backend.API.Models;

internal sealed class Mission
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("planet")]
    public string Planet { get; set; } = string.Empty;

    [BsonElement("rotation")]
    public string Rotation { get; set; } = string.Empty;
}

