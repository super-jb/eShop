using Catalog.API.Data.Entities;

namespace Catalog.API.Commands
{
    public record UpdateProductResponse : CatalogApiResponse
    {
        public Product Product;
    }
}
