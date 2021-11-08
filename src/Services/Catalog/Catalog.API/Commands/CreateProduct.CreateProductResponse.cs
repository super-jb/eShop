using Catalog.API.Data.Entities;

namespace Catalog.API.Commands
{
    public record CreateProductResponse : CatalogApiResponse
    {
        public Product Product { get; set; }
    }
}
