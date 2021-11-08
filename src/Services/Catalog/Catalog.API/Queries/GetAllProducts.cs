using Ardalis.GuardClauses;
using Catalog.API.Data.Entities;
using Catalog.API.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Queries
{
    public class GetAllProducts : IRequestHandler<GetAllProductsQuery, GetAllProductsResponse>
    {
        private readonly IProductRepository _repository;

        public GetAllProducts(IProductRepository repository)
        {
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<GetAllProductsResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await _repository.GetProducts();

            return new GetAllProductsResponse { Products = products };
        }
    }
}
