WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSettings(builder.Configuration);
builder.Services.ConfigureMongoDb(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    await DatabaseInitializer.InitializeAsync(dbInitializer).ConfigureAwait(false);
}

app.MapGet("/", () => "Hello World!");

await app.RunAsync().ConfigureAwait(false);
