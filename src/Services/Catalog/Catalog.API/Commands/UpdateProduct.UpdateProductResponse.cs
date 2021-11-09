using Catalog.API.Data.Entities;

namespace Catalog.API.Commands
{
    public record UpdateProductResponse : JsonApiResponse
    {
        public Product Product;
    }
}
