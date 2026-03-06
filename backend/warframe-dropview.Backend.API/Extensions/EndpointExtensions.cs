using MongoDB.Bson;
using MongoDB.Driver;
using AspNet = Microsoft.AspNetCore.Http;

namespace warframe_dropview.Backend.API.Extensions;

/// <summary>
/// Provides extension methods for mapping API endpoints to an endpoint route builder.
/// </summary>
internal static class EndpointExtensions
{

    /// <summary>
    /// Adds search endpoints for relics, mods, and prime parts to the endpoint route builder.
    /// </summary>
    internal static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/relics/search", HandleRelicsSearch);
        app.MapGet("/api/mods/search", HandleModsSearch);
        app.MapGet("/api/primeparts/search", HandlePrimePartsSearch);
    }

    private static async Task<AspNet.IResult> HandleRelicsSearch([AsParameters] RelicsSearchQuery query, ISender mediator)
    {
        OperationResult<SearchResultDto> result = await mediator.Send(query).ConfigureAwait(false);
        return result.IsSuccess ? Results.Ok(result.Content) : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<AspNet.IResult> HandleModsSearch([AsParameters] ModsSearchQuery query, ISender mediator)
    {
        OperationResult<SearchResultDto> result = await mediator.Send(query).ConfigureAwait(false);
        return result.IsSuccess ? Results.Ok(result.Content) : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<AspNet.IResult> HandlePrimePartsSearch([AsParameters] PrimePartsSearchQuery query, ISender mediator)
    {
        OperationResult<SearchResultDto> result = await mediator.Send(query).ConfigureAwait(false);
        return result.IsSuccess ? Results.Ok(result.Content) : Results.BadRequest(result.ErrorMessage);
    }

    internal static void MapHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/test", HandleHealthCheckEndpoint);
    }

    private static async Task<AspNet.IResult> HandleHealthCheckEndpoint(IMongoDatabase db)
    {
        await db.RunCommandAsync((Command<BsonDocument>)"{ ping: 1 }").ConfigureAwait(false);
        await Task.Delay(100).ConfigureAwait(false);

        return Results.Ok($"Cluster state: {db.Client.Cluster.Description.State.ToString()}");
    }
}