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

    private static async Task<AspNet.IResult> HandleRelicsSearch(RelicsSearchQuery query, ISender mediator)
    {
        OperationResult<SearchResultDto> result = await mediator.Send(query).ConfigureAwait(false);
        return result.IsSuccess ? Results.Ok(result.Content) : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<AspNet.IResult> HandleModsSearch(ModsSearchQuery query, ISender mediator)
    {
        OperationResult<SearchResultDto> result = await mediator.Send(query).ConfigureAwait(false);
        return result.IsSuccess ? Results.Ok(result.Content) : Results.BadRequest(result.ErrorMessage);
    }

    private static async Task<AspNet.IResult> HandlePrimePartsSearch(PrimePartsSearchQuery query, ISender mediator)
    {
        OperationResult<SearchResultDto> result = await mediator.Send(query).ConfigureAwait(false);
        return result.IsSuccess ? Results.Ok(result.Content) : Results.BadRequest(result.ErrorMessage);
    }
}