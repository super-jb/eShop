using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface ICatalogService
{
    Task<IEnumerable<CatalogModel>> GetCatalogs();
    
    Task<CatalogModel> GetCatalogById(string id);

    Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category);

    Task<IEnumerable<CatalogModel>> GetCatalogByName(string name);
}
