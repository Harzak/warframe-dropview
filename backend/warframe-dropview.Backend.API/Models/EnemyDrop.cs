namespace warframe_dropview.Backend.API.Models;

internal sealed class EnemyDrop : Drop
{
    [BsonElement("enemy")]
    public Enemy Enemy { get; set; } = new();
}