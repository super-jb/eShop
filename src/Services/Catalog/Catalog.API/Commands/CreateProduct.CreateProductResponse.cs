using Catalog.API.Data.Entities;

namespace Catalog.API.Commands
{
    public record CreateProductResponse : JsonApiResponse
    {
        public Product Product { get; set; }
    }
}
