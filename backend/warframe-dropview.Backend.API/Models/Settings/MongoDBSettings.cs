namespace warframe_dropview.Backend.API.Models.Settings;

internal sealed class MongoDBSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}