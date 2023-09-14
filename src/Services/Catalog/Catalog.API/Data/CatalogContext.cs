using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration configuration; // Use this to get configuration settings in appsettings.json

        public CatalogContext(IConfiguration configuration)
        {
            this.configuration = configuration;

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionStrings"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));

            CatalogContextSeed.Seed(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
