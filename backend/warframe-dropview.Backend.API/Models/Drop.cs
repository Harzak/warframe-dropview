namespace warframe_dropview.Backend.API.Models;

internal abstract class Drop
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("dropRate")]
    public double DropRate { get; set; }

    [BsonElement("rarity")]
    public string Rarity { get; set; } = string.Empty;
}