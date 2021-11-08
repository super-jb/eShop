using Catalog.API.Data.Entities;
using System.Collections.Generic;

namespace Catalog.API.Queries
{
    public record GetProductByCategoryResponse
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
