using Basket.API.Repositories;
using Basket.API.Repositories.Interface;
using Discount.GRPC.GrpcServices;
using Discount.GRPC.Protos;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to Redis Docker Container
builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString"));

// Inject Repository
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

//Inject Discount.GRPC Client and Service
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    // (options => options.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")));
    (options => options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

builder.Services.AddScoped<DisccountGrpcService>();

// Inject MassTransit
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
// builder.Services.AddMassTransitHostedService(); Not needed in Masstransit v8 and later

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
