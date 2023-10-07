using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Inject Ocelot
builder.Services.AddOcelot().AddCacheManager(settings => settings.WithDictionaryHandle());

// Ocelot Routing Configs
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();
