namespace warframe_dropview.Backend.API.Interfaces;

internal interface IDrop
{
    string Id { get; set; }
    DateTime Timestamp { get; set; }
    string Name { get; set; }
    string Type { get; set; }
    string Subtype { get; set; }
    double DropRate { get; set; }
    string Rarity { get; set; }
}