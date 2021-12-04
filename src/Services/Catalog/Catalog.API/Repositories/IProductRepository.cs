using Catalog.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories;

public interface IProductRepository
{
    // Read operations
    Task<IEnumerable<Product>> GetProducts();
        
    Task<Product> GetProduct(string id);
        
    Task<IEnumerable<Product>> GetProductByName(string name);
        
    Task<IEnumerable<Product>> GetProductByCategory(string categoryName);

    // Write operations
    Task CreateProduct(Product product);
        
    Task<bool> UpdateProduct(Product product);
        
    Task<bool> DeleteProduct(string id);
}
