using System.Text.Json.Serialization;

namespace warframe_dropview.Backend.API.Extensions;

/// <summary>
/// Provides source-generated JSON serialization metadata for application-specific DTO types using System.Text.Json.
/// </summary>
[JsonSerializable(typeof(BaseDropDto))]
[JsonSerializable(typeof(EnemyDropDto))]
[JsonSerializable(typeof(MissionDropDto))]
[JsonSerializable(typeof(RelicDropDto))]
[JsonSerializable(typeof(SearchResultDto))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public partial class AppJsonSerializerContext : JsonSerializerContext
{

}