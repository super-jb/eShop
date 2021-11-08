using Catalog.API.Data.Entities;

namespace Catalog.API.Queries
{
    public record GetProductByIdResponse
    {
        public Product Product { get; set; }
    }
}
