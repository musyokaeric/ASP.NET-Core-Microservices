using AspnetRunBasics.Services.Interfaces;
using AspnetRunBasics.Services;

var builder = WebApplication.CreateBuilder(args);

// Register Http Client Factory
builder.Services.AddHttpClient<ICatalogService, CatalogService>(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
builder.Services.AddHttpClient<IBasketService, BasketService>(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));
builder.Services.AddHttpClient<IOrderService, OrderService>(c => c.BaseAddress = new Uri(builder.Configuration["ApiSettings:GatewayAddress"]));

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
//HostConfigurationSeed.SeedDatabase(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
