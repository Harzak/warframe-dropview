WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterMongoDBPlugin(builder.Configuration)
                .AddEndpointsApiExplorer();

WebApplication app = builder.Build();


app.MapGet("/", () => "Hello World!");

await app.RunAsync().ConfigureAwait(false);
