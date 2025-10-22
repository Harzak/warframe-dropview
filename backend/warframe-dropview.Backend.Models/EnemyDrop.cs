namespace warframe_dropview.Backend.Models;

public sealed class EnemyDrop : Drop
{
    public Enemy Enemy { get; set; } = new();
}