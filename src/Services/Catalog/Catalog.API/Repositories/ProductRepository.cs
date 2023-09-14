using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext context;

        public ProductRepository(ICatalogContext context)
        {
            this.context = context;
        }

        /// ******************************************************
        /// ****************** MONGO DB QUERIES ******************
        /// ******************************************************

        public async Task<IEnumerable<Product>> GetProducts() => await context.Products.Find(p => true).ToListAsync();

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await context.Products.Find(filter).ToListAsync();
        }

        public async Task<Product> GetProduct(string id) => await context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateProduct(Product product) => await context.Products.InsertOneAsync(product);

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            var delete = await context.Products.DeleteOneAsync(filter);

            return delete.IsAcknowledged && delete.DeletedCount > 0;

        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var update = await context.Products.ReplaceOneAsync(
                filter: p => p.Id == product.Id,
                replacement: product);

            return update.IsAcknowledged && update.ModifiedCount > 0;
        }
    }
}
