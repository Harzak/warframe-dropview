using warframe_dropview.Backend.API.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterMongoDBPlugin(builder.Configuration)
                .AddMediatR(config => config.RegisterServicesFromAssemblyContaining<Program>())
                .ConfigureHttpJsonOptions(options =>
                {
                    options.SerializerOptions.TypeInfoResolver = AppJsonSerializerContext.Default;
                })
                .AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.MapSearchEndpoints();

await app.RunAsync().ConfigureAwait(false);
