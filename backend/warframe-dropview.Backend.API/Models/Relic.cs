namespace warframe_dropview.Backend.API.Models;

internal sealed class Relic
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("refinement")]
    public string Refinement { get; set; } = string.Empty;
}