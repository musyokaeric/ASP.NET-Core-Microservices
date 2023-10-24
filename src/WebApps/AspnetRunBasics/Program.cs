using AspnetRunBasics.Data;
using AspnetRunBasics.Repositories;
using AspnetRunBasics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region database services

//// use in-memory database
//builder.Services.AddDbContext<AspnetRunContext>(c =>
//    c.UseInMemoryDatabase("AspnetRunConnection"));

// add database dependecy
builder.Services.AddDbContext<AspnetRunContext>(c =>
    c.UseSqlServer(builder.Configuration.GetConnectionString("AspnetRunConnection")));

#endregion

#region project services

// add repository dependecy
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

#endregion

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
HostConfigurationSeed.SeedDatabase(app);

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
