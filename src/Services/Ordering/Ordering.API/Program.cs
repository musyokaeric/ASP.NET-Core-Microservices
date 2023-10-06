using EventBus.Message.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inject Ordering Project Reference Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Inject MassTransit-RabbitMQ
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((context, configuration) =>
    {
        configuration.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        configuration.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c=>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(context);
        });
    });
});

// Inject AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Inject BasketCheckoutConsumer
builder.Services.AddScoped<BasketCheckoutConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<OrderContext>((context,services)=>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();
});

app.Run();
